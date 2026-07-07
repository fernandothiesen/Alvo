namespace Domain.Entities;

public class ClienteContato
{
    public int IdCliente { get; private set; }
    public int IdContato { get; private set; }

    public Cliente? Cliente { get; private set; }
    public Contato? Contato { get; private set; }

    protected ClienteContato() { }

    public ClienteContato(int idCliente, int idContato)
    {
        IdCliente = idCliente;
        IdContato = idContato;
    }
}