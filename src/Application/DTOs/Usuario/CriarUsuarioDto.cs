using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;



namespace Application.DTOs.Usuario;

public class CriarUsuarioDto
{
    [Required(ErrorMessage = "O nome e obrigatorio")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
    public string Nome {get; set;} = string.Empty;

    [Required(ErrorMessage = "Email e obrigatorio")]
    [EmailAddress(ErrorMessage = "O email nao e valido")]
    public string Email {get; set;} = string.Empty;

    [Required(ErrorMessage = "Senha e obrigatorio")]
    [MinLength(6, ErrorMessage = "Senha deve ter no minimo 6 caracteres")]
    public string Senha {get; set;} = string.Empty;

    [Required(ErrorMessage = "Confirmar senha e obrigatorio")]
    [Compare("Senha", ErrorMessage = "As senhas nao coincidem")]
    public string ConfirmarSenha {get; set;} = string.Empty;


    //Roles opcionais na criacao 
    public List<int> IdsRoles {get; set;} = new();

}