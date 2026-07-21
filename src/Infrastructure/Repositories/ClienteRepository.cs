using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ClienteRepository : Repository<Cliente>, IClienteRepository
{
    public ClienteRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Cliente>> ObterAtivosAsync()
    {
        return await _dbSet
            .Include(c => c.Cidade)
                .ThenInclude(ci => ci!.Estado)
            .Where(c => c.Ativo)
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    public async Task<Cliente?> ObterPorNomeAsync(string nome)
    {
        return await _dbSet
            .Include(c => c.Cidade)
            .FirstOrDefaultAsync(c => c.Nome == nome);
    }

    public async Task<bool> NomeExisteAsync(string nome, int? idClienteExcluir = null)
    {
        var query = _dbSet.Where(c => c.Nome == nome);

        if (idClienteExcluir.HasValue)
            query = query.Where(c => c.IdCliente != idClienteExcluir.Value);

        return await query.AnyAsync();
    }

    // Override para incluir Cidade na listagem
    public new async Task<IEnumerable<Cliente>> ObterTodosAsync()
    {
        return await _dbSet
            .Include(c => c.Cidade)
                .ThenInclude(ci => ci!.Estado)
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    // Override para incluir Cidade ao buscar por ID
    public new async Task<Cliente?> ObterPorIdAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Cidade)
                .ThenInclude(ci => ci!.Estado)
            .FirstOrDefaultAsync(c => c.IdCliente == id);
    }
}