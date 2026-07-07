using Domain.Exceptions;

namespace Domain.Entities;


public class EventoCliente
{
    public int IdEvento {get; private set;}
    public int IdCliente {get; private set;}

    public Evento? Evento {get; private set;}
    public Cliente? Cliente {get; private set;}

    protected EventoCliente(){}

    public EventoCliente(int idEvento, int idCliente)
    {
        idEvento = idEvento;
        idCliente = idCliente;
    }
}