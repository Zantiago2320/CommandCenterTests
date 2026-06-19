using CommanCenter.API.Domain.Entities;

namespace CommanCenter.API.Domain.Interfaces;

public interface IConsultorRepository : IRepository<Consultor>
{
    Task<IEnumerable<Consultor>> GetHabilitadosAsync();
    Task<IEnumerable<Consultor>> GetDeshabilitadosAsync();
    Task<IEnumerable<Consultor>> GetByCelulaAsync(int celulaId);
    Task<Consultor?> GetByEmailAsync(string email);
    Task<IEnumerable<Consultor>> GetCumpleaniosHoyAsync();
    Task<IEnumerable<Consultor>> GetCumpleaniosDelMesAsync(int mes);

    /// <summary>Obtiene un consultor por ID incluyendo sus relaciones de células.</summary>
    Task<Consultor?> GetByIdWithCelulasAsync(int id);
}