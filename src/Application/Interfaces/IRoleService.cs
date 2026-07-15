using Application.DTOs.Role;


namespace Application.Interfaces;


public interface IRoleService
{
    Task<IEnumerable<RoleDto>> ObterTodasAsync();
}