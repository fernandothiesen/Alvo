using Application.DTOs.Auth;
using Application.DTOs.Response;
using Application.DTOs.Usuario;

namespace Application.DTOs.Interfaces;
public interface IAuthService
{
    Task<ResponseResult> LoginAsync(LoginDto dto);
}