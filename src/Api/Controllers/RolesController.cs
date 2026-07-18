using Application.DTOs.Role;
using Application.DTOs.Usuario;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



[ApiController]
[Route("api/[controller]")]
[Authorize] //requer autenticacao

public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;


    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    ///    Lista todas as roles do sistema
    

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterTodas()
    {
        var roles =  await _roleService.ObterTodasAsync();
        return Ok(roles);
    }   

}
