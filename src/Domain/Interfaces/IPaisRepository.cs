using Domain.Entities;
namespace Domain.Interfaces;



public interface IPaisRepository : IRepository<Pais>
{
    Task<Pais?> ObterCodigoAsync(int codigo);
    Task<bool> CodigoExisteAsync(string codigoIso, int? idPaisExcluir = null);
}