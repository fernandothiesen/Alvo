using Application.DTOs.Usuario;
using Application.DTOs.Response;

namespace Application.Interfaces;  
public interface IUsuarioService
{
    Task<ResponseResult> CriarUsuarioAsync (CriarUsuarioDto dto);
    Task<UsuarioDto?> ObterPorId (int id);
    Task<IEnumerable<UsuarioDto>> ObterTodosAsync();
    Task<ResponseResult> AtualizarAsync(int id, CriarUsuarioDto dto);
    Task<ResponseResult> DesativarAsync(int id);
}