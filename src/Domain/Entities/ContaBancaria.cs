using Domain.Exceptions;

namespace Domain.Entities;


public class ContaBancaria
{
    public int IdContaBancaria {get; private set;}
    public string Banco {get; private set;}
    public string Agencia {get; private set;}
    public string NumeroConta {get; private set;}
    public string TipoConta {get; private set;}
    public string Titular {get; private set;}
    public bool Ativo {get; private set;}

    protected ContaBancaria(){}


    public ContaBancaria(string banco, string agencia, string numeroConta, string tipoConta, string titular)
    {
        ValidarBanco(banco);
        ValidarAgencia(agencia);
        ValidarNumeroConta(numeroConta);
        ValidarTipoConta(tipoConta);
        ValidarTitular(titular);



        Banco = banco.Trim();
        Agencia =agencia.Trim();
        NumeroConta = numeroConta.Trim();
        TipoConta = tipoConta.Trim();
        Titular = titular.Trim();
    }



    private void ValidarBanco(string banco)
    {
        if(string.IsNullOrWhiteSpace(banco))
        {
            throw new DomainException("Banco é obrigatorio");
        }

        if(banco.Trim().Length > 100)
            throw new DomainException("Banco deve ser menor que 100 caracteres");
    }


    private void ValidarAgencia(string agencia)
    {
        if(string.IsNullOrWhiteSpace(agencia))
            throw new DomainException("Agencia é obirgatório");

        if(agencia.Trim().Length > 100)
            throw new DomainException("Agencia nao deve ter mais que 100 caracteres");
    }

    public void Ativar()
    {
        Ativo = true;
    }

    public void Desativar()
    {
        Ativo = false;
    }

    public void Atualizar(string banco, string agencia, string numeroConta, 
        string tipoConta, string titular)
    {
        ValidarBanco(banco);
        ValidarAgencia(agencia);
        ValidarNumeroConta(numeroConta);
        ValidarTipoConta(tipoConta);
        ValidarTitular(titular);

        Banco = banco.Trim();
        Agencia = agencia.Trim();
        NumeroConta = numeroConta.Trim();
        TipoConta = tipoConta.Trim();
        Titular = titular.Trim();
    }


    private void ValidarNumeroConta(string numeroConta)
    {
        if (string.IsNullOrWhiteSpace(numeroConta))
            throw new DomainException("Número da conta é obrigatório");

        if (numeroConta.Trim().Length > 20)
            throw new DomainException("Número da conta deve ter no máximo 20 caracteres");
    }

    private void ValidarTipoConta(string tipoConta)
    {
        if (string.IsNullOrWhiteSpace(tipoConta))
            throw new DomainException("Tipo de conta é obrigatório");

        if (tipoConta.Trim().Length > 50)
            throw new DomainException("Tipo de conta deve ter no máximo 50 caracteres");
    }

    private void ValidarTitular(string titular)
    {
        if (string.IsNullOrWhiteSpace(titular))
            throw new DomainException("Titular é obrigatório");

        if (titular.Trim().Length > 150)
            throw new DomainException("Titular deve ter no máximo 150 caracteres");
    }

}