using System;
using Domain.Entities;

namespace Domain.Interfaces;
public interface IUsuario : IRepository<Cliente>
{
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<IEnumerable<Usuario>> ObterAtivosAsync();
    Task<bool> EmailExistenteAsync(string email, int? idUsuarioExcluir = null);
}