namespace Application.DTOs.Estado;


public class EstadoDTO
{
    public int IdEstado {get; set;}
    public int IdPais {get; set;}
    public string NomePais {get; set;} = string.Empty;
    public string NomeEstado {get; set;} = string.Empty;
    public string SiglaEstado {get; set;} = string.Empty;
}