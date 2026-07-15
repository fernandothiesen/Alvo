namespace Application.DTOs.Role;



public class RoleDto
{
    public int IdRole {get; set;}
    public string NomeRole {get; set;} = string.Empty;
    public string? Descricao {get; set;}
}