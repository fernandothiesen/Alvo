using System.ComponentModel.DataAnnotations;


namespace Application.DTO
{
    public class UsuarioDto
    {
        [Required(ErrorMessage = "Nome e obrigatorio")]
        [StringLength(100, MinimumLength = 3)]
        public string Nome {get; set;}

        [Required(ErrorMessage = "Email eh obirgatorio")]
        [EmailAddress]

        public string Email {get; set;}

        [Required(ErrorMessage = "Senha e obirgatorio")]
        [MinLength(6, ErrorMessage = "Senha deve ter no minimo 6 caracteres")]
        public string Senha {get; set;}

        [Compare("Senha", ErrorMessage = "As senhas nao coincidem")]
        public string ConfirmarSenha {get; set;}


    }


}