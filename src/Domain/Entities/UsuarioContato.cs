using Entities;

namespace Domain.Entities;

public class UsuarioContato
{
    public int IdUsuario {get; private set;}
    public int IdContato {get; private set;}


    public Usuario? Usuario {get; private set;}
    public Contato? Contato {get; set;}

    protected UsuarioContato(){}

    public UsuarioContato(int idUsuario, int idContato)
    {
        IdUsuario = idUsuario;
        IdContato = idContato;
    }
}