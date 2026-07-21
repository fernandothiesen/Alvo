using Domain.Entities;
namespace Domain.Interfaces;

public interface IEstadoRepository : IRepository<Estado>
{
    Task<IEnumerable<Estado>> ObterPorPaisAsync(int idPais);
    Task<Estado?> ObterPorSiglaAsync(int idPais, string sigla);
    Task<bool> SiglaExisteNoPaisAsync(int idPais, string sigla, int? idEstadoExcluir = null);
}