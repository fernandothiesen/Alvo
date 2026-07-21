using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DemandaRepository : Repository<Demanda>, IDemandaRepository
{
    public DemandaRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Demanda?> ObterPorIdCompletoAsync(int idDemanda)
    {
        return await _dbSet
            .Include(d => d.Evento)
            .Include(d => d.Clientes).ThenInclude(dc => dc.Cliente)
            .Include(d => d.Fornecedores).ThenInclude(df => df.Fornecedor)
            .FirstOrDefaultAsync(d => d.IdDemanda == idDemanda);
    }

    public async Task<IEnumerable<Demanda>> ObterPorEventoAsync(int idEvento)
    {
        return await _dbSet
            .Include(d => d.Evento)
            .Where(d => d.IdEvento == idEvento)
            .OrderByDescending(d => d.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<Demanda>> ObterPorStatusAsync(string status)
    {
        return await _dbSet
            .Where(d => d.Status == status)
            .OrderByDescending(d => d.DataCriacao)
            .ToListAsync();
    }

    // Override para incluir o Evento na listagem geral
    public new async Task<IEnumerable<Demanda>> ObterTodosAsync()
    {
        return await _dbSet
            .Include(d => d.Evento)
            .OrderByDescending(d => d.DataCriacao)
            .ToListAsync();
    }
}