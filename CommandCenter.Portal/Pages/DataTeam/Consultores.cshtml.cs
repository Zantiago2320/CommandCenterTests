using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommandCenter.Portal.Models;
using CommandCenter.Portal.Services;

namespace CommandCenter.Portal.Pages.DataTeam;

[Authorize]
public class ConsultoresModel : PageModel
{
    private readonly IApiClient _api;

    public List<ConsultorViewModel> Consultores { get; set; } = [];
    public string? Error { get; set; }

    public ConsultoresModel(IApiClient api) => _api = api;

    public async Task OnGetAsync()
    {
        var token = HttpContext.Session.GetString("jwt_token");
        var result = await _api.GetAsync<List<ConsultorViewModel>>("api/consultores", token);

        if (result?.Exitoso == true && result.Data is not null)
            Consultores = result.Data;
        else
            Error = result?.Mensaje ?? "No se pudieron cargar los consultores.";
    }
}
