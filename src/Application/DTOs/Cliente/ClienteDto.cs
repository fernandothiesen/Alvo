namespace Application.DTOs.Cliente;

public class ClienteDTO
{
    public int IdCliente { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    
    // Dados da cidade (achatados para o frontend)
    public int? IdCidade { get; set; }
    public string? NomeCidade { get; set; }
    public string? SiglaEstado { get; set; }
}