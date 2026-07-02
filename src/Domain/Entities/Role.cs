using Domain.Exceptions;

namespace Domain.Entities;
public class Role
{
    private readonly List<RolePermissao> _permissoes = new();


    public int IdRole {get; private set;}
    public string NomeRole {get; private set;}
    public string Descricao {get; private set;}

    public IReadOnlyCollection<RolePermissao> Permissoes => _permissoes.AsReadOnly();

    protected Role(){}


    public Role(string nomeRole, string? descricao = null)
    {

        ValidarNomeRole(nomeRole);


        NomeRole = nomeRole.Trim();
        Descricao = descricao?.Trim();

    }


}