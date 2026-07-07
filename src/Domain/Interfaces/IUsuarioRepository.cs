using System;
using Domain.Entities;

namespace Domain.Interfaces;
public interface IUsuario
{
    Task<Usuario?> ObterPorIdAsync(int id);
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<IEnumerable<Usuario>> ObterTodosAsync();
    Task<IEnumerable<Usuario>> ObterAtivosAsync();
    Task<bool> EmailExistenteAsync(string email, int? idUsuarioExcluir = null);


    //Comandos 

    Task AdicionrUsuarioAsync(Usuario usuario);
    Task AtualizarUsuarioAsync(Usuario usuario);
    Task RemoverUsuarioAsync(int idUsuario);
}