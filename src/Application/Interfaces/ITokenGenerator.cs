namespace Application.Interfaces;
using Domain.Entities;


public interface ITokenGenerator
{
    string GenerateToken(Usuario usuario, List<string> roles, List<string> permissoes);
}