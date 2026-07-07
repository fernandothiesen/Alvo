namespace Domain.Entities;

public class FornecedorDocumento
{
    public int IdFornecedor { get; private set; }
    public int IdDocumento { get; private set; }

    public Fornecedor? Fornecedor { get; private set; }
    public Documento? Documento { get; private set; }

    protected FornecedorDocumento() { }

    public FornecedorDocumento(int idFornecedor, int idDocumento)
    {
        IdFornecedor = idFornecedor;
        IdDocumento = idDocumento;
    }
}