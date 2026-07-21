using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Estado;

public class AtualizarEstadoDTO
{
    // O IdPais geralmente não é alterado, mas se quiser permitir, mantenha-o.
    // Para simplificar, vamos focar em Nome e Sigla.
    
    [Required(ErrorMessage = "O nome do estado é obrigatório")]
    [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
    public string NomeEstado { get; set; } = string.Empty;

    [Required(ErrorMessage = "A sigla do estado é obrigatória")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "A sigla deve ter exatamente 2 caracteres")]
    public string SiglaEstado { get; set; } = string.Empty;
}