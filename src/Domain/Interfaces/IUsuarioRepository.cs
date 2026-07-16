using System;
using Domain.Entities;

namespace Domain.Interfaces;
public interface IUsuario : IRepository<Usuario>
{
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<Usuario?> ObterPorEmailComRolesEPermissoesAsync(string email);

    //Task<IEnumerable<Usuario>> ObterTodosAsync();
    Task<Usuario?> ObterPorIdComRolesAsync(int id);
    Task<IEnumerable<string>> ObterPermissoesPorUsuarioAsync(int idUsuario); // <-- NOVO
    Task<bool> EmailExisteAsync(string email, int? idUsuarioExcluir = null);
    Task AtualizarRolesAsync(Usuario usuario, List<UsuarioRole> novasRoles);
}