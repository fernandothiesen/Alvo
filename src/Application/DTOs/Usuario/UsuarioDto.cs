using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Usuario;

public class UsuarioDto
{
    public int IdUsuario {get; set;}
    public string Nome {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public bool Ativo {get; set;}
    public DateTime DataCriacao {get; set;}
    public DateTime? UltimoLogin {get; set;}
    public List<string> Roles {get; set;} = new();
}