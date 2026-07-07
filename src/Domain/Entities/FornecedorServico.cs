namespace Domain.Entities;

public class FornecedorServico
{
    public int IdFornecedor { get; private set; }
    public int IdServico { get; private set; }

    public Fornecedor? Fornecedor { get; private set; }
    public Servico? Servico { get; private set; }

    protected FornecedorServico() { }

    public FornecedorServico(int idFornecedor, int idServico)
    {
        IdFornecedor = idFornecedor;
        IdServico = idServico;
    }
}