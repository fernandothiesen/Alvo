using Domain.Entities;

namespace Domain.Interfaces;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<IEnumerable<Cliente>> ObterAtivosAsync();
    Task<Cliente?> ObterPorNomeAsync(string nome);
    Task<bool> NomeExisteAsync(string nome, int? idClienteExcluir = null);
}