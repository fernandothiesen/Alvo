namespace Domain.Entities;

public class FornecedorContaBancaria
{
    public int IdFornecedor { get; private set; }
    public int IdConta { get; private set; }

    public Fornecedor? Fornecedor { get; private set; }
    public ContaBancaria? Conta { get; private set; }

    protected FornecedorContaBancaria() { }

    public FornecedorContaBancaria(int idFornecedor, int idConta)
    {
        IdFornecedor = idFornecedor;
        IdConta = idConta;
    }
}