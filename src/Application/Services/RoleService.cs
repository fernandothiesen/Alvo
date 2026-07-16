using Application.Interfaces;
using Application.DTOs.Role;
using Domain.Interfaces;
using Domain.Entities;
using Application.DTOs.Response;


namespace Application.Services;

public class RoleService : IRoleService
{
    private readonly IRepository<Role> _roleRepository;


    public RoleService(IRepository<Role> repository)
    {
        _roleRepository = repository;
    }



    public async Task<IEnumerable<RoleDto>> ObterTodasAsync()
    {
        var roles = await _roleRepository.ObterTodosAsync();

        return roles.Select(r => new RoleDto
        {
            IdRole = r.IdRole,
            NomeRole = r.NomeRole,
            Descricao = r.Descricao
        });
    }

}