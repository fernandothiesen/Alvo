using System.Dynamic;
using Domain.Exceptions;

namespace Domain.Entities;

public class Contato
{
    public int IdContato {get; private set;}
    public string Tipo_Contato {get; private set;}
    public string Valor_Contato {get; private set;}
    public string? Observacao {get; private set;}
    public bool? Ativo {get; private set;}



    protected Contato(){}


    public Contato(string tipo_contato, string valor_contato, string? observacao = null)
    {
        ValidarTipoContato(tipo_contato);
        ValidarValorContato(tipo_contato);

        Tipo_Contato = tipo_contato.Trim();
        Valor_Contato = valor_contato.Trim();
        Observacao = observacao;
    }


    public void ValidarTipoContato(string tipo_contato)
    {
        if(string.IsNullOrWhiteSpace(tipo_contato))
            throw new DomainException("Tipo do contato e obrigatorio");
        if(tipo_contato.Trim().Length > 50)
            throw new DomainException("Contato nao pode ultrapassar 100 caracatres");
    }

    public void ValidarValorContato(string valor_contato)
    {
        if(string.IsNullOrWhiteSpace(valor_contato))
            throw new DomainException("Valor do contato e obrigatorio");
        if(valor_contato.Trim().Length > 50)
            throw new DomainException("Tipo de contato deve ter no maximo 50 caracteres");
    }

    public void Ativar()
    {
        Ativo = true;
    }

    public void Desativar()
    {
        Ativo = false;
    }


    public void Atualizar(string tipo_contato, string valor_contato, string? observacao)
    {
        ValidarTipoContato(tipo_contato);
        ValidarValorContato(valor_contato);

        Tipo_Contato = tipo_contato;
        Valor_Contato = valor_contato;
        Observacao = observacao;
    }


}