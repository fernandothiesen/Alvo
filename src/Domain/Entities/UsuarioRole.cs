using Entities;

namespace Domain.Entities;

public class UsuarioRole
{
    public int IdUsuario { get; private set; }
    public int IdRole { get; private set; }

    //navigation properties
    public Usuario? Usuario { get; private set; }
    public Role? Role { get; private set; }

    protected UsuarioRole() { }

    public UsuarioRole(int idUsuario, int idRole)
    {
        IdUsuario = idUsuario;
        IdRole = idRole;
    }
}