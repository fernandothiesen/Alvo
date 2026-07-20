using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.DTOs.Evento;
public class CriarEventoDto
{
    [Required(ErrorMessage = "O codigo do evento e obrigatorio")]
    [MaxLength(50, ErrorMessage = "O codigo deve ter no maximo 50 caracteres")]
    public string CodigoEvento {get; set;} = string.Empty;

    [Required(ErrorMessage = "O Titulo do evento e obirgatorio")]
    [MaxLength(200, ErrorMessage = "O titulo do evento nao pode ter mais que 200 caracteres")]
    public string Titulo {get; set;} = string.Empty;

    [MaxLength(100)]
    public string? Descricao {get; set;}

    public DateOnly? DataInicio {get; set;}

    public DateOnly? DataFim {get; set;}

    [MaxLength(200)]
    public string? Local {get; set;}

    [MaxLength(50)]
    public string? MesReferencia {get; set;}

    public DateOnly? LimiteEntregaMaterial {get; set;}

    [MaxLength]
    public string? Status {get; set;}

    [MaxLength(1000)]
    public string? Observacoes {get; set;}

    public int? IdCidade {get; set;}
    public int? IdResponsavel {get; set;}
    
    //Relacionamentos opicionais na criacao 

    public List<int>? IdsClientes {get; set;}
    public List<int>? IdsFornecedores {get; set;}
}