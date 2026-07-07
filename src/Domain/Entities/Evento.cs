using Domain.Exceptions;

namespace Domain.Entities; 



public class Evento
{
    private readonly List<EventoCliente> _clientes = new();
    private readonly List<EventoFornecedor> _fornecedores = new();
    private readonly List<Demanda> _demandas = new();
    private readonly List<Financeiro> _financeiros = new();



    public int IdEvento {get; private set;}
    public string CodigoEvento {get; private set;}
    public string Titulo {get; private set;}
    public string? Descricao {get; private set;}
    public DateOnly? DataInicio {get; private set;}
    public DateOnly? DataFim {get; private set;}
    public string? Local {get; private set;}
    public string? MesReferencia {get; private set;}
    public DateOnly? LimiteEntregaMaterial {get; private set;}
    public string? Status {get; private set;}
    public string? Observacoes {get; private set;}
    public int? IdCidade {get; private set;}
    public int? IdUsuarioResponsavel {get; private set;}



    public Cidade? Cidade { get; private set; }
    public Usuario? Responsavel { get; private set; }
    public IReadOnlyCollection<EventoCliente> Clientes => _clientes.AsReadOnly();
    public IReadOnlyCollection<EventoFornecedor> Fornecedores => _fornecedores.AsReadOnly();
    public IReadOnlyCollection<Demanda> Demandas => _demandas.AsReadOnly();
    public IReadOnlyCollection<Financeiro> Financeiros => _financeiros.AsReadOnly();

    protected Evento(){}


    public Evento(string codigoEvento, string titulo, string? descricao = null, int? idCidade = null, int? idResponsavel = null)
    {
        ValidarCodigoEvento(codigoEvento);
        ValidarTitulo(Titulo);


        CodigoEvento = codigoEvento.Trim();
        Titulo = titulo.Trim();
        IdCidade = idCidade;
        IdUsuarioResponsavel = idResponsavel;
    }


    public void Atulizar(string codigoEvento, string titulo, string? descricao, DateOnly? dataInicio, DateOnly? dataFim, 
    string? local, string? mesReferencia, DateOnly? limiteEntregaMaterial, string? status, string? observacoes, int? idCidade, 
    int idResponsavel)
    {
        ValidarCodigoEvento(codigoEvento);
        ValidarTitulo(titulo);
        ValidarDatas(dataInicio, dataFim);

        CodigoEvento = codigoEvento.Trim();
        Titulo = titulo.Trim();
        DataInicio = dataInicio;
        DataFim = dataFim;
        Local = local?.Trim();
        MesReferencia = mesReferencia?.Trim();
        LimiteEntregaMaterial = limiteEntregaMaterial;
        Status = status?.Trim();
        Observacoes = observacoes?.Trim();
        IdCidade = idCidade;
        IdUsuarioResponsavel = idResponsavel;
    }



    public void AdicionarCliente (EventoCliente eventoCliente)
    {
        if(_clientes.Any(c => c.idCliente == eventoCliente.idCliente))
            throw new DomainException("Esse evento ja existe para esse cliente");

        _clientes.Add(eventoCliente);
    }

    public void RemoverCliente(int idCliente)
    {
        var clienteRemover = _clientes.FirstOrDefault(c => c.IdCliente == idCliente);


        if(clienteRemover == null)
            throw new DomainException("Cliente nao encontrado para esse evento");
    }

    public void AdicionarFornecedor(EventoFornecedor eventoFornecedor)
    {
        if(_fornecedores.Any(f => f.IdFornecedor == eventoFornecedor.IdFornecedor))
            throw new DomainException("Evento ja possui este fornecedor");

        _clientes.Add(eventoFornecedor);
    }


    public void RemoverFornecedor(int idFornecedor)
    {
        var fornecedorRemover = _fornecedores.FirstOrDefault(f => f.IdForneceodor == idFornecedor);

        if(fornecedorRemover == null)
        {
            throw new DomainException("Fornecedor nao existe para esse evento");
        }


        _fornecedores.Remove(fornecedorRemover);
    }


    private void ValidarCodigoEvento(string codigoEvento)
    {
        if (string.IsNullOrWhiteSpace(codigoEvento))
            throw new DomainException("Código do evento é obrigatório");

        if (codigoEvento.Trim().Length > 50)
            throw new DomainException("Código do evento deve ter no máximo 50 caracteres");
    }

    private void ValidarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("Título do evento é obrigatório");

        if (titulo.Trim().Length < 3)
            throw new DomainException("Título do evento deve ter no mínimo 3 caracteres");

        if (titulo.Trim().Length > 200)
            throw new DomainException("Título do evento deve ter no máximo 200 caracteres");
    }


    private void ValidarDatas(DateOnly? dataInicio, DateOnly? dataFim)
    {
        if(dataInicio.HasValue && dataFim.HasValue && dataInicio > dataFim)
        {
            throw new DomainException("Data de inicio nao pode ser maior que a data fim");
        }
    }
}