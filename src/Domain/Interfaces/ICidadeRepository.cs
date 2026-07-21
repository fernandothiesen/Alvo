using Domain.Entities;

namespace Domain.Interfaces;

public interface ICidadeRepository : IRepository<Cidade>
{
    Task<IEnumerable<Cidade>> ObterPorEstadoAsync(int idEstado);
    Task<Cidade?> ObterPorNomeAsync(int idEstado, string nome);
    Task<bool> NomeExisteNoEstadoAsync(int idEstado, string nome, int? idCidadeExcluir = null);
}