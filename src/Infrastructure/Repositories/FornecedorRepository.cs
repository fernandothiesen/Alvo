using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
{
    public FornecedorRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Fornecedor>> ObterAtivosAsync()
    {
        return await _dbSet
            .Include(f => f.Cidade)
                .ThenInclude(ci => ci!.Estado)
            .Where(f => f.Ativo)
            .OrderBy(f => f.Nome)
            .ToListAsync();
    }

    public async Task<Fornecedor?> ObterPorNomeAsync(string nome)
    {
        return await _dbSet
            .Include(f => f.Cidade)
            .FirstOrDefaultAsync(f => f.Nome == nome);
    }

    public async Task<bool> NomeExisteAsync(string nome, int? idFornecedorExcluir = null)
    {
        var query = _dbSet.Where(f => f.Nome == nome);

        if (idFornecedorExcluir.HasValue)
            query = query.Where(f => f.IdFornecedor != idFornecedorExcluir.Value);

        return await query.AnyAsync();
    }

    // Override para incluir Cidade na listagem
    public new async Task<IEnumerable<Fornecedor>> ObterTodosAsync()
    {
        return await _dbSet
            .Include(f => f.Cidade)
                .ThenInclude(ci => ci!.Estado)
            .OrderBy(f => f.Nome)
            .ToListAsync();
    }

    // Override para incluir Cidade ao buscar por ID
    public new async Task<Fornecedor?> ObterPorIdAsync(int id)
    {
        return await _dbSet
            .Include(f => f.Cidade)
                .ThenInclude(ci => ci!.Estado)
            .FirstOrDefaultAsync(f => f.IdFornecedor == id);
    }
}