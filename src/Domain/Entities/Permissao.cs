using Domain.Exceptions;

namespace Domain.Entities;

public class Permissao
{
    public int IdPermissao {get; private set;}
    public string NomePermissao {get; private set;}
    public string? Descricao {get; private set;}

    protected Permissao(){} 

    public Permissao(string nomePermissao, string? descricao = null)
    {
        ValidarNomePermissao(nomePermissao);

        NomePermissao = nomePermissao.Trim();
        Descricao = descricao.Trim();
    }

    public void Atualizar(string nomePermissao, string? descricao)
    {
        ValidarNomePermissao(nomePermissao);

        NomePermissao = nomePermissao.Trim();
        Descricao = descricao.Trim();
    }


    private void ValidarNomePermissao(string nomePermissao)
    {
        if(string.IsNullOrWhiteSpace(nomePermissao))
            throw new DomainException("Nome e obirgatorio");

        if(nomePermissao.Trim().Length < 3)
            throw new DomainException("Nome da permissao deve ter no minimo 3 caracteres");

        if(nomePermissao.Trim().Length > 100)
            throw new DomainException("Nome da permissao deve ter no maximo 100 caracteres");
    }

}