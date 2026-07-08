using Domain.Entities;

namespace Domain.Interfaces;


public interface IDemandaRepository : IRepository<Demanda>
{
    Task<IEnumerable<Demanda>> ObterPorEventoAsync(int idEvento);
    Task<IEnumerable<Demanda>> ObterPorStatusAsync(string status);
}