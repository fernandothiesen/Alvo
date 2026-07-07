namespace Domain.Entities;

public class DemandaFornecedor
{
    public int IdDemanda { get; private set; }
    public int IdFornecedor { get; private set; }

    public Demanda? Demanda { get; private set; }
    public Fornecedor? Fornecedor { get; private set; }

    protected DemandaFornecedor() { }

    public DemandaFornecedor(int idDemanda, int idFornecedor)
    {
        IdDemanda = idDemanda;
        IdFornecedor = idFornecedor;
    }
}