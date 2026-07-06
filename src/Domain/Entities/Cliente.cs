using Domain.Exceptions;

namespace Domain.Entities;


public class Cliente
{
   private readonly List<ClienteContato> _contatos = new();
   private readonly List<ClienteDocumento> _documentos = new();
   private readonly List<Eventocliente> _eventos = new();
   private readonly List<DemandaCliente> _demandas = new();

   public int IdCliente {get; private set;}
   public string Nome {get; private set;}
   public bool Ativo {get; private set;}
   public DateTime DataCadastro {get; private set;}
   public int? IdCidade {get; private set;}


   public Cidade? Cidade {get; private set;}
   public IReadOnlyCollection<ClienteContato> Contatos => _contatos.AsReadOnly();
   public IReadOnlyCollection<ClienteDocumento> Documentos => _documentos.AsReadOnly();
   public IReadOnlyCollection<EventoCliente> Eventos => _eventos.AsReadOnly();
   public IReadOnlyCollection<DemandaCliente> Demandas => _demandas.AsReadOnly();


   protected Cliente(){}


    public Cliente(string nome, int? idCidade)
    {
        ValidarNome(nome);

        Nome = nome.Trim();
        IdCidade = idCidade;
        Ativo = true;
        DataCadastro = DateTime.UtcNow;
    }


    public void Atualizar(string nome, int? idCidade)
    {
        ValidarNome(nome);

        Nome = nome.Trim();
        IdCidade = idCidade;
    }



    public void Ativar()
    {
        Ativo = true;
    }

    public void Desativar()
    {
        Ativo = false;
    }


    private void ValidarNome(string nome)
    {
        if(string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do cliente e obrigatorio");
        if(nome.Trim().Length < 3)
            throw new DomainException("Nome do cliente deve ter no minimo 3 caracteres");
        if(nome.Trim().Length > 150)
            throw new DomainException("Nome do cliente nao pode possuir mais que 150 caracteres");
    }


    public void AdicionarContato(ClienteContato clienteContato)
    {
        if(_contatos.Any(c => c.IdContato == clienteContato.IdContato))
            throw new DomainException("Cliente ja possui este contato");
        
        _contatos.Add(clienteContato);
    }

    public void RemoverContato(int idContato)
    {
        var contatoRemover = _contatos.FirstOrDefault(c => c.IdContato == idContato);

        if(contatoRemover == null)
            throw new DomainException("Esse contato nao existe para esse cliente");

        _contatos.Remove(contatoRemover);
    }


    public void AdicionarDocumento(ClienteDocumento clienteDocumento)
    {
        if(_documentos.Any(d => d.IdDocumento == clienteDocumento.IdDocumento))
            throw new DomainException("Esse documento ja existe para esse cliente");

        _documentos.Add(clienteDocumento);
    }

    public void RemoverDocumento(int idDocumento)
    {
        var clienteDocumento = _documentos.FirstOrDefault(d => d.IdDocumento == idDocumento);

        if(clienteDocumento == null)
            throw new DomainException("Documento nao existe para esse cliente");

        _documentos.Remove(clienteDocumento);
    }
}