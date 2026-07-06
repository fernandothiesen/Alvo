using Domain.Exceptions;


namespace Domain.Entities;



public class Servico
{
    public int IdServico {get; private set;}
    public string NomeServico {get; private set;}
    public string? Descricao {get; private set;}


    protected Servico() {}


    public Servico(string nomeServico, string? descricao = null)
    {
        NomeServico = nomeServico.Trim();
        Descricao = descricao?.Trim();
    }


    public void Atualizar(string nomeServico, string? descricao)
    {
        ValidarNomeServico(nomeServico);

        NomeServico =  nomeServico.Trim();
        Descricao = descricao?.Trim();
    }



    private void ValidarNomeServico(string nomeServico)
    {
        if(string.IsNullOrWhiteSpace(nomeServico))
            throw new DomainException(nomeServico);

        if(nomeServico.Trim().Length < 3)
            throw new DomainException("Nome do servico deve ser mairo que 3 caracteres");

        if(nomeServico.Trim().Length > 150)
            throw new DomainException("Nome do servico nao pode ultrapassar 150 caracteres");
    }


}