namespace Application.DTOs.Cidade;

public class CidadeDTO
{
    public int IdCidade { get; set; }
    public int IdEstado { get; set; }
    public string NomeEstado { get; set; } = string.Empty; // Para facilitar exibição
    public string SiglaEstado { get; set; } = string.Empty; // Bônus para o frontend
    public string NomeCidade { get; set; } = string.Empty;
}