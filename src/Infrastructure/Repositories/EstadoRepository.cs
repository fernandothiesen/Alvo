using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EstadoRepository : Repository<Estado>, IEstadoRepository
{
    public EstadoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Estado>> ObterPorPaisAsync(int idPais)
    {
        return await _dbSet
            .Where(e => e.IdPais == idPais)
            .OrderBy(e => e.NomeEstado)
            .ToListAsync();
    }

    public async Task<Estado?> ObterPorSiglaAsync(int idPais, string sigla)
    {
        return await _dbSet
            .FirstOrDefaultAsync(e => e.IdPais == idPais && e.SiglaEstado == sigla.ToUpper());
    }

    public async Task<bool> SiglaExisteNoPaisAsync(int idPais, string sigla, int? idEstadoExcluir = null)
    {
        var siglaNormalizada = sigla.Trim().ToUpper();
        var query = _dbSet.Where(e => e.IdPais == idPais && e.SiglaEstado == siglaNormalizada);

        if (idEstadoExcluir.HasValue)
            query = query.Where(e => e.IdEstado != idEstadoExcluir.Value);

        return await query.AnyAsync();
    }

    // Override para incluir o País no mapeamento e ordenar
    public new async Task<IEnumerable<Estado>> ObterTodosAsync()
    {
        return await _dbSet
            .Include(e => e.Pais)
            .OrderBy(e => e.Pais!.NomePais)
            .ThenBy(e => e.NomeEstado)
            .ToListAsync();
    }

    // Override para incluir o País ao buscar por ID
    public new async Task<Estado?> ObterPorIdAsync(int id)
    {
        return await _dbSet
            .Include(e => e.Pais)
            .FirstOrDefaultAsync(e => e.IdEstado == id);
    }
}