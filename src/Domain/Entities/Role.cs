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


    public void Atualizar(string nomeRole, string? descricao)
    {
        ValidarNomeRole(nomeRole);

        NomeRole = nomeRole.Trim();
        Descricao = descricao?.Trim();
    }


    public void AdicionarPermissao(RolePermissao rolePermissao)
    {
        if(_permissoes.Any(p => p.IdPermissao == rolePermissao.IdPermissao))
            throw new DomainException("Role ja possui esta permissao");

        _permissoes.Add(rolePermissao);
    }

    public void RemoverPermissao(int idPermissao)
    {
        var permissao = _permissoes.FirstOrDefault(p => p.IdPermissao == idPermissao);

        if(permissao == null)
            throw new DomainException("Permissao nao encontrada para esta role");

        _permissoes.Remove(permissao);
    }


    private void ValidaNomeRole(string nomeRole)
    {
        if(string.IsNullOrWhiteSpace(nomeRole))
            throw new DomainException("Nome da role e obrigatorio");

        if(nomeRole.Trim().Length < 3)
        {
            throw new DomainException("Nome da role deve ter no minimo 3 caracteres");
        }

        if(nomeRole.Trim().Length > 50)
        {
            throw new DomainException("Nome da role deve ter no maximo 50 caracteres");
        }
    }

}