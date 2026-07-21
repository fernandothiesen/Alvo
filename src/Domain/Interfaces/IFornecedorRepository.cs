using Domain.Entities;

namespace Domain.Interfaces;

public interface IFornecedorRepository : IRepository<Fornecedor>
{
    Task<IEnumerable<Fornecedor>> ObterAtivosAsync();
    Task<Fornecedor?> ObterPorNomeAsync(string nome);
   Task<bool> NomeExisteAsync(string nome, int? idFornecedorExcluir = null);

}