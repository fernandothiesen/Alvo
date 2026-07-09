using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "Email e obrigatorio")]
    [EmailAddress(ErrorMessage = "O email nao e valido")]
    public string Email {get; set;} = string.Empty;


    [Required(ErrorMessage = "senha e obrigatorio")]
    public string Senha {get; set;} = string.Empty;
}