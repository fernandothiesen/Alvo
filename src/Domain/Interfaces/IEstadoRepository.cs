using Domain.Entities;
namespace Domain.Interfaces;

public interface IEstadoRepository : IRepository<Estado>
{
    Task<IEnumerable<Estado>> ObterPaisAsync(int idPais);
    Task<Estado> ObterSiglaAsync(string sigla);
}