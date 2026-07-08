using Domain.Entities;

namespace Domain.Interfaces;


public interface ICidadeRepository : IRepository<Cidade>
{
    Task<IEnumerable<Cliente>> ObterAtivosAsync();
    Task<bool> NomeExistenteAsync(string nome, int? idFornecedorExcluir = null);
}