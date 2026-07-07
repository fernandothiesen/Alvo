using Domain.Exceptions;


namespace Domain.Entities;


public class Financeiro
{
    public int IdFinanceiro {get; private set;}
    public string Tipo {get; private set;}
    public decimal Valor {get; private set;}
    public DateOnly DataMovimentacao {get; private set;}
    public string? Descricao {get; private set;}
    public int? IdEvento {get; private set;}
    public int? IdDemanda {get; private set;}
    public int? IdFornecedor {get; private set;}
    public int? IdCliente {get; private set;}
    public int IdFormaPagamento {get; private set;}
    public int? IdConta {get; private set;}

    public Evento? Evento {get; private set;}
    public Demanda? Demanda {get; private set;}
    public Fornecedor? Fornecedor {get; private set;}
    public Cliente? Cliente {get; private set;}
    public FormaPagamento? FormaPagamento {get; private set;}
    public ContaBancaria? ContaBancaria {get; private set;}


    protected Financeiro(){}


    public Financeiro(string tipo, decimal valor, DateOnly dataMovimentacao, int idFormaPagamento, string? descricao = null, 
    int? idEvento = null, int? idDemanda = null, int? idFornecedor = null, int? idCliente = null, int? idConta = null)
    {
        ValidarTipo(tipo);
        ValidarValor(valor);
        ValidarIdFormaPagamento(idFormaPagamento);


        Tipo = tipo.Trim();
        Valor = valor;
        DataMovimentacao = dataMovimentacao;
        IdFormaPagamento = idFormaPagamento;
        Descricao = descricao?.Trim();
        IdEvento = idEvento;
        IdDemanda = idDemanda;
        IdFornecedor = idFornecedor;
        IdCliente = idCliente;
        IdConta = idConta;
    }



    public void Atualizar(string tipo, decimal valor, DateOnly dataMovimentracao, int idFormaPagamento, string? descricao, int? idEvento,
    int? idDemanda, int? idFornecedor, int? idCliente, int? idConta)
    {
        ValidarTipo(tipo);
        ValidarValor(valor);
        ValidarIdFormaPagamento(idFormaPagamento);

        Tipo = tipo.Trim();
        Valor = valor;
        DataMovimentacao = dataMovimentacao;
        IdFormaPagamento = idFormaPagamento;
        Descricao = descricao?.Trim();
        IdEvento = idEvento;
        IdDemanda = idDemanda;
        IdFornecedor = idFornecedor;
        IdCliente = idCliente;
        IdConta = idConta;

    }

    private void ValidarTipo(string tipo)
    {
        if(string.IsNullOrWhiteSpace(tipo))
            throw new DomainException("Tipo de movimentacao é obrigatorio");

        if(tipo.Trim().Length > 50)
            throw new DomainException("Tipo de movimentação deve ter no máximo 50 caracteres");
    }

    private void ValidarValor(decimal valor)
    {
        if(valor <= 0)
            throw new DomainException("Valor deve ser maior que zero");

        if(valor > 999999999.99m)
            throw new DomainException("Valor não pode exceder 999.999.999,99");
    }


    private void ValidarIdFormaPagamento(int idFormaPagamento)
    {
        if(idFormaPagamento <= 0)
            throw new DomainException("ID de forma de pagamento deve ser maior que zero");
    }


}