using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Pais;


public class PaisDTO
{   
    public int IdPais {get; set;}
    public string NomePais {get; set;} = string.Empty;
    public string CodigoIso {get; set;} = string.Empty;
}