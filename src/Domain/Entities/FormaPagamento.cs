using Domain.Exceptions;

namespace Domain.Entities;

public class FormaPagamento
{
    public int IdPagamento {get; private set;}
    public string TipoFormaPagamento {get; private set;}
    public string? Descricao {get; private set;}


    protected FormaPagamento(){}

    public FormaPagamento(string tipoFormaPagamento, string? descricao = null)
    {
        TipoFormaPagamento = tipoFormaPagamento.Trim();
        Descricao = descricao?.Trim();
    }


    private void ValidarTipoFormaPagamento(string formaPagamento)
    {
        if(string.IsNullOrWhiteSpace(formaPagamento))
        {
            throw new DomainException("Forma de pagamento é obrigatória");
        }

        if(formaPagamento.Trim().Length > 50)
            throw new DomainException("Tipo de forma de pagamento deve ter no maximo 50 caracteres");
    }

}