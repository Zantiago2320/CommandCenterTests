using Microsoft.AspNetCore.Identity;

var hasher = new PasswordHasher<IdentityUser>();
var user = new IdentityUser();

var passwords = new Dictionary<string, string>
{
    ["admin"] = "Admin@123",
    ["alexander"] = "Alexander@123",
    ["sergio"] = "Sergio@123",
    ["senior"] = "Senior@123"
};

foreach (var kvp in passwords)
{
    var hash = hasher.HashPassword(user, kvp.Value);
    Console.WriteLine($"-- {kvp.Key.ToUpper()}: {kvp.Value}");
    Console.WriteLine($"'{hash}'");
    Console.WriteLine();
}
