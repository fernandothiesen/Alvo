using System;
using Domain.Entities;

namespace Domain.Interfaces;
public interface IUsuario : IRepository<Usuario>
{
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<IEnumerable<Usuario>> ObterAtivosAsync();
    Task<bool> EmailExistenteAsync(string email, int? idUsuarioExcluir = null);
    Task AdicionarAsync (Usuario usuario);
    Task AtualizarAsync (Usuario usuario);
    Task RemoverAsync(int idUsuario);
}