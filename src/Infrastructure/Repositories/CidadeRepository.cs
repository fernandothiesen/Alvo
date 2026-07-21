using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CidadeRepository : Repository<Cidade>, ICidadeRepository
{
    public CidadeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Cidade>> ObterPorEstadoAsync(int idEstado)
    {
        return await _dbSet
            .Where(c => c.IdEstado == idEstado)
            .OrderBy(c => c.NomeCidade)
            .ToListAsync();
    }

    public async Task<Cidade?> ObterPorNomeAsync(int idEstado, string nome)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.IdEstado == idEstado && c.NomeCidade == nome);
    }

    public async Task<bool> NomeExisteNoEstadoAsync(int idEstado, string nome, int? idCidadeExcluir = null)
    {
        var query = _dbSet.Where(c => c.IdEstado == idEstado && c.NomeCidade == nome);

        if (idCidadeExcluir.HasValue)
            query = query.Where(c => c.IdCidade != idCidadeExcluir.Value);

        return await query.AnyAsync();
    }

    // Override para incluir o Estado
    public new async Task<IEnumerable<Cidade>> ObterTodosAsync()
    {
        return await _dbSet
            .Include(c => c.Estado)
            .OrderBy(c => c.Estado!.SiglaEstado)
            .ThenBy(c => c.NomeCidade)
            .ToListAsync();
    }

    // Override para incluir o Estado ao buscar por ID
    public new async Task<Cidade?> ObterPorIdAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Estado)
            .FirstOrDefaultAsync(c => c.IdCidade == id);
    }
}