using Domain.Entities;

namespace Domain.Interfaces;

public interface IFornecedorRepository : IRepository<Fornecedor>
{
    Task<IEnumerable<Fornecedor>> ObterAtivosAsync();
    Task<bool> NomeExisteAsync(string nome, int? idFornecedorExcluir = null);
}