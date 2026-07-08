using Domain.Entities;

namespace Domain.Interfaces;

public interface IContaBancariaRepository : IRepository<ContaBancaria>
{
    Task<IEnumerable<ContaBancaria>> ObterAtivasAsync();
    Task<IEnumerable<ContaBancaria>> ObterPorBancoAsync(string banco);
}