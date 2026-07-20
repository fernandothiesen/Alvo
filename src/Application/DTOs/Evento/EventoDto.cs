using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Evento;

public class EventoDto
{
    public int IdEvento {get; set;}
    public string CodigoEvento {get; set;} = string.Empty;
    public string Titulo {get; set;} = string.Empty;
    public string? Descricao {get; set;}
    public DateOnly? DataInicio {get; set;}
    public DateOnly? DataFim {get; set;}
    public string? Local {get; set;}
    public string? MesReferencia {get; set;}
    public DateOnly? LimiteEntregaMaterial {get; set;}
    public string? Status {get; set;}
    public string? Observacoes {get; set;}


    //Dados relacionados 
    public string? NomeCidade {get; set;}
    public string? NomeResponsavel {get; set;}

    //listas de relacionamentos 

    public List<ItemResumoDto> Clientes {get; set;} = new();
    public List<ItemResumoDto> Fornecedores {get; set;} = new();

}

public class ItemResumoDto
{
    public int Id {get; set;}
    public string Nome {get; set;} = string.Empty;
}