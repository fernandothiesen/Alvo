using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Pais;

public class AtualizarPaisDTO
{
    [Required(ErrorMessage = "O nome do país é obrigatório")]
    [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
    public string NomePais { get; set; } = string.Empty;

    [Required(ErrorMessage = "O código ISO é obrigatório")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "O código ISO deve ter exatamente 3 caracteres")]
    public string CodigoIso { get; set; } = string.Empty;
}