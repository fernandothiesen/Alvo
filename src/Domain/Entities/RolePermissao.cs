using Domain.Exceptions;


namespace Domain.Entities;

public class RolePermissao
{
    public int IdRole {get; private set;}
    public int IdPermissao {get; private set;}

    public Role? Role {get; private set;}
    public Permissao? Permissao {get; private set;}
    protected RolePermissao(){}

    public RolePermissao(int idRole, int idPermissao)
    {
        IdRole = idRole;
        IdPermissao = idPermissao;
    }
}