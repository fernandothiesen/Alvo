using Domain.Entities;
namespace Domain.Interfaces;



public interface IPaisRepository : IRepository<Pais>
{
    Task<Pais?> ObterPorCodigoIsoAsync(string codigoIso);
    Task<bool> CodigoIsoExisteAsync(string codigoIso, int? idPaisExcluir = null);
    Task<IEnumerable<Pais>> ObterAtivosAsync();
}