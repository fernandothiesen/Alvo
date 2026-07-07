namespace Domain.Entities;


public class EventoFornecedor
{
    public int IdEvento {get; private set;}
    public int IdFornecedor {get; private set;}


    public Evento? Evento {get; private set;}
    public Fornecedor? Fornecedor {get; private set;}

    protected EventoFornecedor(){}

    public EventoFornecedor(int idEvento, int idFornecedor)
    {
        IdEvento = idEvento;
        IdFornecedor = idFornecedor;
    }
}