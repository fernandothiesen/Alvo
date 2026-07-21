using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Estado;

public class CriarEstadoDTO
{
    [Required(ErrorMessage = "O ID do país é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O ID do país deve ser válido")]
    public int IdPais { get; set; }

    [Required(ErrorMessage = "O nome do estado é obrigatório")]
    [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
    public string NomeEstado { get; set; } = string.Empty;

    [Required(ErrorMessage = "A sigla do estado é obrigatória")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "A sigla deve ter exatamente 2 caracteres")]
    public string SiglaEstado { get; set; } = string.Empty;
}