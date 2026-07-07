namespace Domain.Entities;

public class FornecedorContato
{
    public int IdFornecedor { get; private set; }
    public int IdContato { get; private set; }

    public Fornecedor? Fornecedor { get; private set; }
    public Contato? Contato { get; private set; }

    protected FornecedorContato() { }

    public FornecedorContato(int idFornecedor, int idContato)
    {
        IdFornecedor = idFornecedor;
        IdContato = idContato;
    }
}