using Domain.Entities;

namespace Domain.Interfaces;

public interface IAtividadeSistemaRepository : IRepository<AtividadeSistema>
{
    Task<IEnumerable<AtividadeSistema>> ObterPorUsuarioAsync(int idUsuario);
    Task<IEnumerable<AtividadeSistema>> ObterPorTipoAsync(string tipoAtividade);
    Task<IEnumerable<AtividadeSistema>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);
}