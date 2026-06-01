using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using CommandCenter.Portal.Services;

namespace CommandCenter.Portal.Pages.Auth;

public class LoginModel : PageModel
{
    private readonly IAuthPortalService _auth;
    private readonly ILogger<LoginModel> _logger;

    [BindProperty] public string Email    { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }

    public LoginModel(IAuthPortalService auth, ILogger<LoginModel> logger)
    {
        _auth = auth;
        _logger = logger;
    }

    public IActionResult OnGet(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToPage("/Index");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (!ModelState.IsValid) return Page();

        var (ok, token, error) = await _auth.LoginAsync(Email, Password);

        if (!ok || token is null)
        {
            ErrorMessage = error ?? "Credenciales incorrectas.";
            return Page();
        }

        // Guardar el token en sesión para usarlo en llamadas posteriores a la API
        HttpContext.Session.SetString("jwt_token", token);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name,  Email),
            new(ClaimTypes.Email, Email)
        };

        var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = false });

        _logger.LogInformation("Usuario {Email} autenticado en el Portal", Email);

        return LocalRedirect(returnUrl ?? "/");
    }
}
