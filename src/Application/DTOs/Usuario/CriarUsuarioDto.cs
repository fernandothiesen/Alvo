using System.ComponentModel.DataAnnotations;
using Domain.Entities;


namespace Application.DTOs.Usuario;

public class CriarUsuarioDto
{

    [Required, MaxLength(100)]
    public string Nome {get; set;}

    [Required, MaxLength(150)]
    public string Email {get; set;}
    [Required]
    public string senha {get; set;}
}