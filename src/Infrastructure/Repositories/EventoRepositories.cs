using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;


public class EventoRepository : Repository<Evento>, IEventoRepository
{
    
    public EventoRepository(ApplicationDbContext context) : base(context){}

      /// <summary>
    /// Busca evento com TODOS os relacionamentos carregados
    /// Usa Eager Loading para evitar N+1 queries
    /// </summary>
    public async Task<Evento?> ObterPorIdCompletoAsync(int idEvento)
    {
        return await _dbSet
            .Include(e => e.Cidade)
                .ThenInclude(c => c!.Estado)
                    .ThenInclude(e => e!.Pais)
            .Include(e => e.Responsavel)
            .Include(e => e.Clientes)
                .ThenInclude(ec => ec.Cliente)
            .Include(e => e.Fornecedores)
                .ThenInclude(ef => ef.Fornecedor)
            .Include(e => e.Demandas)
            .Include(e => e.Financeiros)
            .FirstOrDefaultAsync(e => e.IdEvento == idEvento);
    }

    /// <summary>
    /// Busca evento por código único
    /// </summary>
    public async Task<Evento?> ObterPorCodigoAsync(string codigoEvento)
    {
        return await _dbSet
            .Include(e => e.Cidade)
            .Include(e => e.Responsavel)
            .FirstOrDefaultAsync(e => e.CodigoEvento == codigoEvento);
    }

    /// <summary>
    /// Verifica se um código de evento já existe no banco
    /// </summary>
    public async Task<bool> CodigoExisteAsync(string codigoEvento, int? idEventoExcluir = null)
    {
        var query = _dbSet.Where(e => e.CodigoEvento == codigoEvento);

        if (idEventoExcluir.HasValue)
            query = query.Where(e => e.IdEvento != idEventoExcluir.Value);

        return await query.AnyAsync();
    }

    /// <summary>
    /// Lista eventos filtrados por status
    /// </summary>
    public async Task<IEnumerable<Evento>> ObterPorStatusAsync(string status)
    {
        return await _dbSet
            .Include(e => e.Cidade)
            .Include(e => e.Responsavel)
            .Where(e => e.Status == status)
            .OrderByDescending(e => e.DataInicio)
            .ToListAsync();
    }

    /// <summary>
    /// Lista eventos de um responsável específico
    /// </summary>
    public async Task<IEnumerable<Evento>> ObterPorResponsavelAsync(int idResponsavel)
    {
        return await _dbSet
            .Include(e => e.Cidade)
            .Where(e => e.IdResponsavel == idResponsavel)
            .OrderByDescending(e => e.DataInicio)
            .ToListAsync();
    }

    /// <summary>
    /// Lista eventos de uma cidade específica
    /// </summary>
    public async Task<IEnumerable<Evento>> ObterPorCidadeAsync(int idCidade)
    {
        return await _dbSet
            .Include(e => e.Responsavel)
            .Where(e => e.IdCidade == idCidade)
            .OrderByDescending(e => e.DataInicio)
            .ToListAsync();
    }

    /// <summary>
    /// Override do método genérico para incluir relacionamentos básicos na listagem
    /// </summary>
    public new async Task<IEnumerable<Evento>> ObterTodosAsync()
    {
        return await _dbSet
            .Include(e => e.Cidade)
            .Include(e => e.Responsavel)
            .OrderByDescending(e => e.DataInicio)
            .ToListAsync();
    }

}