using Domain.Exceptions;


namespace Domain.Entities;


public class Fornecedor
{

    private readonly List<FornecedorContato> _contatos = new();
    private readonly List<FornecedorDocumento> _documentos = new();
    private readonly List<EventoFornecedor> _eventos = new();
    private readonly List<DemandaFornecedor> _demandas = new();
    private readonly List<FornecedorContaBancaria> _contasBancarias = new();
    private readonly List<FornecedorServico> _servicos = new();


    public int IdFornecedor {get; private set;}
    public string Nome {get; private set;}
    public bool Ativo {get; private set;}
    public DateTime DataCadastro {get; private set;}
    public int? IdCidade {get; private set;}
    public Cidade? Cidade {get; private set;}


    public IReadOnlyCollection<FornecedorContato> Contatos => _contatos.AsReadOnly();
    public IReadOnlyCollection<FornecedorDocumento> Documentos => _documentos.AsReadOnly();
    public IReadOnlyCollection<EventoFornecedor> Eventos => _eventos.AsReadOnly();
    public IReadOnlyCollection<DemandaFornecedor> Demandas => _demandas.AsReadOnly();
    public IReadOnlyCollection<FornecedorContaBancaria> ContasBancarias => _contasBancarias.AsReadOnly();
    public IReadOnlyCollection<FornecedorServico> Servicos => _servicos.AsReadOnly();


    protected Fornecedor(){}


    public Fornecedor( string nome, int? idCidade = null)
    {

        ValidarNome(nome);


        Nome = nome.Trim();
        Ativo = true;
        IdCidade = idCidade;
        DataCadastro = DateTime.UtcNow;
    }

    public void Ativar()
    {
        Ativo  = true;
    }

    public void Desativar()
    {
        Ativo = false;
    }

    public void Atualizar(string nome, int? idCidade)
    {
        ValidarNome(nome);


        Nome = nome.Trim();
        IdCidade = idCidade;
    }


    private void ValidarNome(string nome)
    {
        if(string.IsNullOrWhiteSpace(nome))
        {
            throw new DomainException("Nome é obrigatorio");
        }

        if(nome.Trim().Length < 3)
            throw new DomainException("Nome do fornecedor nao pode ser menor que 3 caracteres");

        if(nome.Trim().Length > 150)
            throw new DomainException("Nome nao pode ter mais que 150 caracteres");
    }



    public void AdicionarContato(FornecedorContato fornecedorContato)
    {
        if(_contatos.Any(c => c.IdContato == fornecedorContato.IdContato))
        {
            throw new DomainException("Esse contato ja existe para esse fornecedor");
        }

        _contatos.Add(fornecedorContato);
    }


    public void RemoverContato(int idContato)
    {
        var contatoFornecedor = _contatos.FirstOrDefault(c => c.IdContato == idContato);

        if(contatoFornecedor == null)
            throw new DomainException("Esse contato nao existe");

        _contatos.Remove(contatoFornecedor);    
    }


    public void AdicionarDocumento(FornecedorDocumento fornecedorDocumento)
    {
        if(_documentos.Any(d => d.IdDocumento == fornecedorDocumento.IdDocumento))
            throw new DomainException("Esse documento ja existe para esse fornecedor");

        _documentos.Add(fornecedorDocumento);
    }


    public void RemoverDocumento(int idDocumento)
    {
        var documentoRemover = _documentos.FirstOrDefault(d => d.IdDocumento == idDocumento);

        if(documentoRemover == null)
            throw new DomainException("esse documento nao existe");

        _documentos.Remove(documentoRemover);
    }


    public void AdicionarContaBancaria(FornecedorContaBancaria fornecedorContaBancaria)
    {
        if(_contasBancarias.Any(c => c.IdConta == fornecedorContaBancaria.IdConta))
            throw new DomainException("Essa conta bancaria ja existe para esse fornecedor");


        _contasBancarias.Add(fornecedorContaBancaria);
    }


    public void RemoverContaBancaria(int idConta)
    {
        var contaRemover = _contasBancarias.FirstOrDefault(c => c.IdConta == idConta);

        if(contaRemover == null)
            throw new DomainException("Essa conta nao existe");

        _contasBancarias.Remove(contaRemover);
    }

    public void AdicionarServico(FornecedorServico fornecedorServico)
    {
        if (_servicos.Any(s => s.IdServico == fornecedorServico.IdServico))
            throw new DomainException("Fornecedor já possui este serviço");

        _servicos.Add(fornecedorServico);
    }

    public void RemoverServico(int idServico)
    {
        var servico = _servicos.FirstOrDefault(s => s.IdServico == idServico);
        if (servico == null)
            throw new DomainException("Serviço não encontrado para este fornecedor");

        _servicos.Remove(servico);
    }


}