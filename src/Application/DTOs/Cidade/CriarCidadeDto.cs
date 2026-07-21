using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Cidade;

public class CriarCidadeDTO
{
    [Required(ErrorMessage = "O ID do estado é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O ID do estado deve ser válido")]
    public int IdEstado { get; set; }

    [Required(ErrorMessage = "O nome da cidade é obrigatório")]
    [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
    public string NomeCidade { get; set; } = string.Empty;
}