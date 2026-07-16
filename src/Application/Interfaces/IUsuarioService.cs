using Application.DTOs.Usuario;
using Application.DTOs.Response;
using Application.DTOs.Role;
using Application.DTOs.Permissao;


namespace Application.Interfaces;  
public interface IUsuarioService
{
    Task<ResponseResult> CriarUsuarioAsync (CriarUsuarioDto dto);
    Task<UsuarioDto?> ObterPorIdAsync (int id);
    Task<IEnumerable<UsuarioDto>> ObterTodosAsync();
    Task<ResponseResult> AtualizarAsync(int id, CriarUsuarioDto dto);
    Task<ResponseResult> DesativarAsync(int id);
    Task<ResponseResult> AtualizarRolesAsync(int idUsuario, AtualizarRoleDto dto);
    Task<ResponseResult> AtualizarPermissoesAsync(int idUsuario, AtualizarPermissaoDto dto);

}