using System;
using Entities;


public interface IUsuario
{
    Task<Usuario> AdicionarUsuarioAsync(Usuario usuario);
    Task AtualizarUsuarioAsync(Usuario usuario);
    Task<Usuario> DeletarUsuarioAsync(Usuario usuario);
    Task<IEnumerable<Usuario>> ObterTodosAsync();
    Task<Usuario> ListarUsuarioPorIdAsync(int id);
    Task<Usuario> ObterUsuarioPorEmailAsync(string email);
    Task<Usuario> ExisteEmailAsync(string email, int? id_usuario = null);
}