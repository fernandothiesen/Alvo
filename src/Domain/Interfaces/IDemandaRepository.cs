using Domain.Entities;

namespace Domain.Interfaces;

public interface IDemandaRepository : IRepository<Demanda>
{
    Task<Demanda?> ObterPorIdCompletoAsync(int idDemanda);
    Task<IEnumerable<Demanda>> ObterPorEventoAsync(int idEvento);
    Task<IEnumerable<Demanda>> ObterPorStatusAsync(string status);
}