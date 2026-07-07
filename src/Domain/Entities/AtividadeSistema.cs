using Domain.Exceptions;


namespace Domain.Entities;

public class AtividadeSistema
{
    public int IdAtividadeSistema {get; private set;}  
    public int IdUsuario {get; private set;}
    public string TipoAtividade {get; private set;}
    public string? Descricao {get; private set;}
    public DateTime DataHora {get; private set;}


    protected AtividadeSistema(){}

    public AtividadeSistema(int idUsuario, string tipoAtividade, string? descricao = null)
    {
        ValidarIdUsuario(idUsuario);
        ValidarTipoAtividade(tipoAtividade);

        IdUsuario = idUsuario;
        TipoAtividade = tipoAtividade.Trim();
        Descricao = descricao?.Trim();
        DataHora = DateTime.UtcNow;
    }


    private void ValidarIdUsuario(int idUsuario)
    {
        if(idUsuario <= 0)
            throw new DomainException("ID do usuário deve ser maior que zero");
    }

    private void ValidarTipoAtividade(string tipoAtividade)
    {
        if(string.IsNullOrWhiteSpace(tipoAtividade))
            throw new DomainException("Tipo de atividade é obirgatório");

        if(tipoAtividade.Trim().Length > 100)
            throw new DomainException("Tipo de atividade deve ter no maximo 100 caracteres");
    }

}