using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Demanda;

public class AtualizarDemandaDTO
{
    [Required(ErrorMessage = "O título da demanda é obrigatório")]
    [MaxLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres")]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Descricao { get; set; }

    [MaxLength(50)]
    public string? Prioridade { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    public List<int>? IdsClientes { get; set; }
    public List<int>? IdsFornecedores { get; set; }
}