using Domain.Entities;

namespace Domain.Interfaces;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<IEnumerable<Cliente>> ObterAtivosAsync();
    Task<bool> NomeExisteAsync(string nome, int? idClienteExcluir = null);
}