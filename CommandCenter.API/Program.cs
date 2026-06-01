using CommandCenter.API.Extensions;
using CommandCenter.API.Infrastructure.Data;
using CommandCenter.API.Middleware;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ── Serilog ────────────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.File("Logs/commandcenter-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// ── Servicios ──────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentityConfig();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerConfig(builder.Configuration);
builder.Services.AddHangfireConfig(builder.Configuration);
builder.Services.AddCorsConfig(builder.Configuration);
builder.Services.AddApplicationServices();

// Application Insights (habilitar en producción)
if (builder.Configuration.GetValue<bool>("ApplicationInsights:Enabled"))
{
    builder.Services.AddApplicationInsightsTelemetry(
        builder.Configuration["ApplicationInsights:ConnectionString"]);
}

// ── Build ──────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Migración automática + Seed en desarrollo ──────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await db.Database.MigrateAsync();
    await SeedAsync(roleManager, userManager);
}

// ── Middleware Pipeline ────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Command Center API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("CommandCenterPolicy");
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

// Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

// Hangfire Dashboard (solo admins en producción)
app.UseHangfireDashboard(
    builder.Configuration["Hangfire:DashboardPath"] ?? "/jobs",
    new DashboardOptions { Authorization = new[] { new HangfireAuthFilter() } });

app.MapControllers();

Log.Information("🚀 Command Center API iniciada en {Environment}", app.Environment.EnvironmentName);

app.Run();

// ── Seed inicial ───────────────────────────────────────────────────────────
static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
{
    string[] roles = ["SuperAdmin", "Admin", "Lider", "User"];
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    const string adminEmail = "admin@commandcenter.com";
    if (await userManager.FindByEmailAsync(adminEmail) is null)
    {
        var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        await userManager.CreateAsync(admin, "Admin@12345!");
        await userManager.AddToRoleAsync(admin, "SuperAdmin");
        Log.Warning("✅ SuperAdmin creado: {Email} / Admin@12345! — Cambiar en producción", adminEmail);
    }
}
