using Domain.Entities;

namespace Domain.Interfaces;

public interface IContatoRepository : IRepository<Contato>
{
    Task<IEnumerable<Contato>> ObterAtivosAsync();
    Task<IEnumerable<Contato>> ObterPorTipoAsync(string tipoContato);
}