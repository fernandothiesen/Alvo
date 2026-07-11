using Domain.Exceptions;

namespace Domain.Entities;


public class Pais
{

    private readonly List<Estado> _estados = new();
    public int IdPais {get; private set;}
    public string NomePais {get; private set;}

    public string CodigoIso {get; private set;}


    protected Pais(){}

    public Pais(string nome_pais, string codigo_iso)
    {
        ValidarNomePais(nome_pais);
        ValidarCodigoPais(codigo_iso);

        NomePais = nome_pais.Trim();
        CodigoIso = codigo_iso.Trim();
    }



    public void Atualizar(string nome_pais, string codigo_iso)
    {
        ValidarNomePais(nome_pais);
        ValidarCodigoPais(codigo_iso);

        NomePais = nome_pais.Trim();
        CodigoIso = codigo_iso.Trim();
    }

    private void ValidarNomePais(string nome_pais)
    {
        if(string.IsNullOrWhiteSpace(nome_pais))
            throw new DomainException("nome do pais e obirgatorio");
        if(nome_pais.Length < 4)
            throw new DomainException("Pais deve ter no minimo 4 caracters");
        if(nome_pais.Length > 100)
            throw new DomainException("Nome do pais deve ter no maximo 100 caracteres");
    }


    private void ValidarCodigoPais(string codigo_iso)
    {
        if(string.IsNullOrWhiteSpace(codigo_iso))
            throw new DomainException("codigo iso e obrigatorio");
        if(codigo_iso.Length < 2)
            throw new DomainException("codigo iso deve ter no minimo 2 caracteres");
        if(codigo_iso.Length > 3)
            throw new DomainException("codigo iso deve ter no maximo 3 caracteres");
    }
}