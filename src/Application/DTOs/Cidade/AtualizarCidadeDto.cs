using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Cidade;

public class AtualizarCidadeDTO
{
    [Required(ErrorMessage = "O nome da cidade é obrigatório")]
    [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
    public string NomeCidade { get; set; } = string.Empty;
}