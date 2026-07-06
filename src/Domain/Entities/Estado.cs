using System.Linq.Expressions;
using Domain.Exceptions;

namespace Domain.Entities;

public class Estado
{
   private readonly List<Cidade> _cidades = new();

   public int IdEstado {get; private set;}
   public int IdPais {get; private set;}
   public string NomeEstado {get; private set;}
   public string SiglaEstado {get; private set;}

   public Pais? Pais {get; private set;}
   public IReadOnlyCollection<Cidade> Cidades => _cidades.AsReadOnly();

   protected Estado(){}


   public Estado(int idPais, string nomeEstado, string siglaEstado)
   {
        ValidarIdPais(idPais);
        ValidarNomeEstado(nomeEstado);
        ValidarSiglaEstado(siglaEstado);


        IdPais = idPais;
        NomeEstado = nomeEstado.Trim();
        SiglaEstado = siglaEstado.Trim().ToUpper();
   }



    public void Atualizar(string nomeEstado, string siglaEstado)
    {
        ValidarNomeEstado(nomeEstado);
        ValidarSiglaEstado(siglaEstado);

        NomeEstado = nomeEstado.Trim();
        SiglaEstado = siglaEstado.Trim().ToUpper();
    }

    public void ValidarIdPais(int idPais)
    {
        if(idPais <= 0)
            throw new DomainException("ID do pais deve ser maior que zero");
    }

    public void ValidarNomeEstado(string nomeEstado)
    {
        if(string.IsNullOrWhiteSpace(nomeEstado))
            throw new DomainException("Nome do estado e obrigatorio");

        if(nomeEstado.Trim().Length > 100)
            throw new DomainException("Nome do estado deve ter no maximo 100 caracteres");
    }

    public void ValidarSiglaEstado(string siglaEstado)
    {
        if(string.IsNullOrWhiteSpace(siglaEstado))
            throw new DomainException("sigla do estado e obrigatorio");
        if(siglaEstado.Trim().Length != 2)
            throw new DomainException("Sigla do estado deve ter exatamente 2 caracteres");
    }
}