using Domain.Entities;

namespace Domain.Interfaces;

public interface IFormaPagamentoRepository : IRepository<FormaPagamento>
{
    Task<FormaPagamento?> ObterPorTipoAsync(string tipoFormaPagamento);
}