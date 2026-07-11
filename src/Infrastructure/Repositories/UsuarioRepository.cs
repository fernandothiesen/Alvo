using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuario
{
    public UsuarioRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        return await _dbSet
            .Include(u => u.Roles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario?> ObterPorEmailComRolesEPermissoesAsync(string email)
    {
        return await _dbSet
            .Include(u => u.Roles).ThenInclude(ur => ur.Role).ThenInclude(r => r.Permissoes).ThenInclude(rp => rp.Permissao)
            .Include(u => u.Permissoes).ThenInclude(up => up.Permissao)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> EmailExisteAsync(string email, int? idUsuarioExcluir = null)
    {
        var query = _dbSet.Where(u => u.Email == email);
        if (idUsuarioExcluir.HasValue)
            query = query.Where(u => u.IdUsuario != idUsuarioExcluir.Value);

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<string>> ObterRolesPorUsuarioAsync(int idUsuario)
    {
        return await _context.UsuarioRoles
            .Where(ur => ur.IdUsuario == idUsuario)
            .Select(ur => ur.Role!.NomeRole)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> ObterPermissoesPorUsuarioAsync(int idUsuario)
    {
        var permissoesDasRoles = await _context.UsuarioRoles
            .Where(ur => ur.IdUsuario == idUsuario)
            .SelectMany(ur => ur.Role!.Permissoes)
            .Select(rp => rp.Permissao!.NomePermissao)
            .ToListAsync();

        var permissoesDiretas = await _context.UsuarioPermissoes
            .Where(up => up.IdUsuario == idUsuario)
            .Select(up => up.Permissao!.NomePermissao)
            .ToListAsync();

        return permissoesDasRoles.Union(permissoesDiretas).Distinct().ToList();
    }
}