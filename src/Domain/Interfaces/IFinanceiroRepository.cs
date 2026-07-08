using Domain.Entities;

namespace Domain.Interfaces;

public interface IFinanceiroRepository : IRepository<Financeiro>
{
    Task<IEnumerable<Financeiro>> ObterPorTipoAsync(string tipo);
    Task<IEnumerable<Financeiro>> ObterPorEventoAsync(int idEvento);
    Task<IEnumerable<Financeiro>> ObterPorDemandaAsync(int idDemanda);
    Task<decimal> ObterTotalPorTipoAsync(string tipo);
}