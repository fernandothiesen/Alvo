namespace Application.DTOs.Fornecedor;


public class FornecedorDTO
{
    public int IdFornecedor {get; set;}
    public string Nome {get; set;} = string.Empty;
    public bool Ativo {get; set;}
    public DateTime DataCadastro {get; set;}

    // Dados da cidade 

    public int? IdCidade {get; set;}
    public string? NomeCidade {get; set;}
    public string? SiglaEstado {get; set;}

}