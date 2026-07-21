using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Demanda;

public class CriarDemandaDTO
{
    [Required(ErrorMessage = "O ID do evento é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O ID do evento deve ser válido")]
    public int IdEvento { get; set; }

    [Required(ErrorMessage = "O título da demanda é obrigatório")]
    [MaxLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres")]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Descricao { get; set; }

    [MaxLength(50)]
    public string? Prioridade { get; set; }

    // Opcional na criação
    public List<int>? IdsClientes { get; set; }
    public List<int>? IdsFornecedores { get; set; }
}