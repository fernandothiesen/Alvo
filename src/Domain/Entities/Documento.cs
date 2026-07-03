using Domain.Exceptions;

namespace Domain.Entities;


public class Documento
{
    public int IdDocumento {get; private set;}
    public string Tipo_Documento {get; private set; }

    public string Valor_Documento {get; private set;}
    public bool? Ativo {get; private set;}
    public DateOnly? DataValidacao {get; private set;}


    protected Documento(){}

    public Documento(string tipo_documento, string valor_documento)
    {
        ValidarTipoDocumento(tipo_documento);
        ValidarValorDocumento(valor_documento);

        Tipo_Documento = tipo_documento.Trim();
        Valor_Documento = valor_documento.Trim();
        Ativo = true;
    }


    public void Ativar()
    {
        Ativo = true;
    }

    public void Desativar()
    {
        Ativo = false;
    }

   private void ValidarTipoDocumento(string tipoDocumento)
    {
        if (string.IsNullOrWhiteSpace(tipoDocumento))
            throw new DomainException("Tipo de documento é obrigatório");

        if (tipoDocumento.Trim().Length > 50)
            throw new DomainException("Tipo de documento deve ter no máximo 50 caracteres");
    }

    private void ValidarValorDocumento(string valorDocumento)
    {
        if (string.IsNullOrWhiteSpace(valorDocumento))
            throw new DomainException("Valor do documento é obrigatório");

        if (valorDocumento.Trim().Length > 100)
            throw new DomainException("Valor do documento deve ter no máximo 100 caracteres");
    }

    public void Atualizar(string tipo_documento, string valor_documento, DateOnly? dataValidacao)
    {
        ValidarTipoDocumento(tipo_documento);
        ValidarValorDocumento(valor_documento);


        Tipo_Documento = tipo_documento;
        Valor_Documento = valor_documento;
        DataValidacao = dataValidacao;
        
    }
}
