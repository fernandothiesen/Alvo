namespace Domain.Entities;

public class ClienteDocumento
{
    public int IdCliente { get; private set; }
    public int IdDocumento { get; private set; }

    public Cliente? Cliente { get; private set; }
    public Documento? Documento { get; private set; }

    protected ClienteDocumento() { }

    public ClienteDocumento(int idCliente, int idDocumento)
    {
        IdCliente = idCliente;
        IdDocumento = idDocumento;
    }
}