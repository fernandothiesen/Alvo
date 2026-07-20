using Domain.Entities;

namespace Domain.Interfaces;

public interface IEventoRepository : IRepository<Evento>
{
    // Busca evento com todos os relacionamentos carregados
    Task<Evento?> ObterPorIdCompletoAsync(int idEvento);

    // Busca evento por código único
    Task<Evento?> ObterPorCodigoAsync(string codigoEvento);

    // Verifica se código já existe (útil para validação)
    Task<bool> CodigoExisteAsync(string codigoEvento, int? idEventoExcluir = null);

    // Lista eventos com filtros básicos
    Task<IEnumerable<Evento>> ObterPorStatusAsync(string status);

    // Lista eventos de um responsável específico
    Task<IEnumerable<Evento>> ObterPorResponsavelAsync(int idResponsavel);

    // Lista eventos de uma cidade específica
    Task<IEnumerable<Evento>> ObterPorCidadeAsync(int idCidade);
}