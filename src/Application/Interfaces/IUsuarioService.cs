using Application.DTO;
using Entities;

public interface IUsuarioService
{
    Task CriarUsuarioAsync(UsuarioDto usuarioDto);
    Task AtualizarUsuarioAsync(UsuarioDto usuarioDto);
    Task GerarSenhaHashUsuario(UsuarioDto usuarioDto);
    




}