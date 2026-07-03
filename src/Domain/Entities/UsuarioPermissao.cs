using Domain.Exceptions;
using Entities;

namespace Domain.Entities;


public class UsuarioPermissao
{
    public int IdUsuario {get; private set;}
    public int IdPermissao {get; private set;}

    public Usuario? Usuario {get; private set;}
    public Permissao? Permissao {get; private set;}


    protected UsuarioPermissao(){}

    public UsuarioPermissao(int idUsuario, int idPermissao)
    {
        IdUsuario = idUsuario;
        IdPermissao = idPermissao;
    }
}