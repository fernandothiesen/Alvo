using Application.DTOs.Evento; // Reutilizando o ItemResumoDTO

namespace Application.DTOs.Demanda;

public class DemandaDTO
{
    public int IdDemanda { get; set; }
    public int IdEvento { get; set; }
    public string TituloEvento { get; set; } = string.Empty; // Para facilitar exibição
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Prioridade { get; set; } // Ex: "Baixa", "Média", "Alta", "Urgente"
    public string? Status { get; set; }     // Ex: "Pendente", "Em Andamento", "Concluída"
    public DateTime DataCriacao { get; set; }
    public DateTime? DataConclusao { get; set; }

    // Relacionamentos N:N
    public List<ItemResumoDto> Clientes { get; set; } = new();
    public List<ItemResumoDto> Fornecedores { get; set; } = new();
}