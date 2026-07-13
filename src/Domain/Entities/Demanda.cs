using Domain.Exceptions;

namespace Domain.Entities;


public class Demanda
{
    private readonly List<DemandaCliente> _clientes = new();
    private readonly List<DemandaFornecedor> _fornecedores = new();
    private readonly List<Financeiro> _financeiros = new();


    public int IdDemanda {get; private set;}
    public int IdEvento {get; private set;}
    public string Titulo {get; private set;}
    public string? Descricao {get; private set;}
    public string? Prioridade {get; private set;}
    public string? Status {get; private set;}
    public DateTime DataCriacao {get; private set;}
    public DateTime? DataConclusao {get; private set;}


    public Evento? Evento {get; private set;}
    public IReadOnlyCollection<DemandaCliente> Clientes => _clientes.AsReadOnly();
    public IReadOnlyCollection<DemandaFornecedor> Fornecedores => _fornecedores.AsReadOnly();
    public IReadOnlyCollection<Financeiro> Financeiros => _financeiros.AsReadOnly();


    protected Demanda(){}

   public Demanda(int idEvento, string titulo, string? descricao = null, string? prioridade = null)
    {
        ValidarIdEvento(idEvento);
        ValidarTitulo(titulo);

        IdEvento = idEvento;
        Titulo = titulo.Trim();
        Descricao = descricao?.Trim();
        Prioridade = prioridade?.Trim();
        Status = "Pendente";
        DataCriacao = DateTime.UtcNow;
    }

    public void Atualizar(string titulo, string? descricao, string? prioridade, string? status)
    {
        ValidarTitulo(titulo);

        Titulo = titulo.Trim();
        Descricao = descricao?.Trim();
        Prioridade = prioridade?.Trim();
        Status = status?.Trim();
    }

    public void Concluir()
    {
        Status = "Concluida";
        DataConclusao = DateTime.UtcNow;
    }

    public void AdicionarCliente(DemandaCliente demandaCliente)
    {
        if (_clientes.Any(c => c.IdCliente == demandaCliente.IdCliente))
            throw new DomainException("Demanda já possui este cliente");

        _clientes.Add(demandaCliente);
    }

    public void RemoverCliente(int idCliente)
    {
        var cliente = _clientes.FirstOrDefault(c => c.IdCliente == idCliente);
        if (cliente == null)
            throw new DomainException("Cliente não encontrado para esta demanda");

        _clientes.Remove(cliente);
    }

    public void AdicionarFornecedor(DemandaFornecedor demandaFornecedor)
    {
        if (_fornecedores.Any(f => f.IdFornecedor == demandaFornecedor.IdFornecedor))
            throw new DomainException("Demanda já possui este fornecedor");

        _fornecedores.Add(demandaFornecedor);
    }

    public void RemoverFornecedor(int idFornecedor)
    {
        var fornecedor = _fornecedores.FirstOrDefault(f => f.IdFornecedor == idFornecedor);
        if (fornecedor == null)
            throw new DomainException("Fornecedor não encontrado para esta demanda");

        _fornecedores.Remove(fornecedor);
    }


    private void ValidarIdEvento(int idEvento)
    {
        if(idEvento <= 0)  
            throw new DomainException("ID do evento deve ser maior que zero");
    }


    private void ValidarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("Título da demanda é obrigatório");

        if (titulo.Trim().Length < 3)
            throw new DomainException("Título da demanda deve ter no mínimo 3 caracteres");

        if (titulo.Trim().Length > 200)
            throw new DomainException("Título da demanda deve ter no máximo 200 caracteres");
    }
}