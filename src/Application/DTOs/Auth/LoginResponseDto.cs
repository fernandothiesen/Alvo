using Application.DTOs.Usuario;

namespace Application.DTOs.Auth;



public class LoginResponseDto
{
    public string Token {get; set;} = string.Empty;
    public DateTime Expiracao {get; set;}
    public UsuarioDto Usuario {get; set;} = new();
}