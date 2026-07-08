using Domain.Entities;

namespace Domain.Interfaces;


public interface IEventoRepository : IRepository<Evento>
{
    Task<Evento?> ObterCodigoAsync(string codigoEvento);
    Task<bool> CodigoExisteAsync(string codigoEvento, int? idEventoExcluir);
    Task<IEnumerable<Evento>> ObterPorStatusAsync(string status);
    Task<IEnumerable<Evento>> ObterPorResponsavelAsync(int idResponsavel); 
}