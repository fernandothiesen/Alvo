namespace Domain.Entities;

public class DemandaCliente
{
    public int IdDemanda { get; private set; }
    public int IdCliente { get; private set; }

    public Demanda? Demanda { get; private set; }
    public Cliente? Cliente { get; private set; }

    protected DemandaCliente() { }

    public DemandaCliente(int idDemanda, int idCliente)
    {
        IdDemanda = idDemanda;
        IdCliente = idCliente;
    }
}