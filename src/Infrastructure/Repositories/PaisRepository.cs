using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PaisRepository : Repository<Pais>, IPaisRepository
{
    public PaisRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Pais?> ObterPorCodigoIsoAsync(string codigoIso)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.CodigoIso == codigoIso.ToUpper());
    }

    public async Task<bool> CodigoIsoExisteAsync(string codigoIso, int? idPaisExcluir = null)
    {
        var codigoNormalizado = codigoIso.Trim().ToUpper();
        var query = _dbSet.Where(p => p.CodigoIso == codigoNormalizado);

        if (idPaisExcluir.HasValue)
            query = query.Where(p => p.IdPais != idPaisExcluir.Value);

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Pais>> ObterAtivosAsync()
    {
        // País não tem campo "ativo", então retornamos todos ordenados
        return await _dbSet
            .OrderBy(p => p.NomePais)
            .ToListAsync();
    }

    // Override para retornar ordenado por nome
    public new async Task<IEnumerable<Pais>> ObterTodosAsync()
    {
        return await _dbSet
            .OrderBy(p => p.NomePais)
            .ToListAsync();
    }
}