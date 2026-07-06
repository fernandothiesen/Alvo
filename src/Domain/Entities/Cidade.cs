using Domain.Exceptions;

namespace Domain.Entities;

public class Cidade
{
    public int IdCidade {get; private set;}
    public int IdEstado {get; private set;}
    public string NomeCidade {get; private set;}

    public Estado? Estado {get; private set;}

    protected Cidade(){}


    public Cidade(int idEstado, string nomeCidade)
    {
        ValidarIdEstado(idEstado);
        ValidarNomeCidade(nomeCidade);


        IdEstado = idEstado;
        NomeCidade = nomeCidade.Trim();
    }


    public void Atualizar(string nomeCidade)
    {
        ValidarNomeCidade(nomeCidade);

        NomeCidade = nomeCidade;
    }

    private void ValidarIdEstado(int idEstado)
    {
        if(idEstado <= 0)
            throw new DomainException("ID do estado deve ser maior que zero");
    }

    private void ValidarNomeCidade(string nomeCidade)
    {
        if(string.IsNullOrWhiteSpace(nomeCidade))
            throw new DomainException("Nome do estado e obrigatorio");
        if(nomeCidade.Trim().Length > 100)
            throw new DomainException("Nome da cidade nao pode possuir mais que 100 caracteres");
    }





}