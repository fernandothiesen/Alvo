using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Evento;

public class AtualizarEventoDTO
{
    [Required(ErrorMessage = "O título do evento é obrigatório")]
    [MaxLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres")]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Descricao { get; set; }

    public DateOnly? DataInicio { get; set; }

    public DateOnly? DataFim { get; set; }

    [MaxLength(200)]
    public string? Local { get; set; }

    [MaxLength(50)]
    public string? MesReferencia { get; set; }

    public DateOnly? LimiteEntregaMaterial { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    [MaxLength(1000)]
    public string? Observacoes { get; set; }

    public int? IdCidade { get; set; }

    // Permite atualizar a lista de clientes e fornecedores do evento
    public List<int>? IdsClientes { get; set; }
    public List<int>? IdsFornecedores { get; set; }
}
