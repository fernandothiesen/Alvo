using Domain.Entities;

namespace Domain.Interfaces;


public interface IServicoRepository : IRepository<Servico>
{
    Task<Servico?> ObterNomeAsync(string nome);
}