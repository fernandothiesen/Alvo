using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Pais;


public class CriarPaisDTO
{
    [Required(ErrorMessage = "Nome do pais eh obrigatorio")]
    [MaxLength(100, ErrorMessage = "Pais nao deve ter mais que 100 caracteres")]
    public string NomePais {get; set;} = string.Empty;

    [Required(ErrorMessage = "Codigo ISO do pais eh obrigatorio")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "O codigo da ISO deve ter exatamente 3 caracteres")]
    public string CodigoIso {get; set;} = string.Empty;
}