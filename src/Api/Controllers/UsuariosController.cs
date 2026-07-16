using Application.DTOs.Role;
using Application.DTOs.Usuario;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize] //autenticacao para todos os endpoints


public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ILogger<UsuariosController> _logger;


    public UsuariosController(IUsuarioService usuarioService, ILogger<UsuariosController> logger)
    {
        _usuarioService = usuarioService;
        _logger = logger;
    }


    // <summary>
    //Obtem usuarios pelo ID
    // <summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterPorId (int id)
    {
        _logger.LogInformation("Buscando usuario com ID: {Id}", id);

        var usuario = await _usuarioService.ObterPorIdAsync(id);

        if(usuario == null)
        {
            _logger.LogInformation("Usuario com ID {Id} nao encontrado", id);
            return  NotFound(new { message = "Usuario nao encomntrado"});
        }
    
        return Ok(usuario);
    } 


    /// <summary>
    /// Obtem todos os usuarios
    /// <sumary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterTodos()
    {
        _logger.LogInformation("Listando todos os usuarios");

        var usuarios = await _usuarioService.ObterTodosAsync();
        return Ok(usuarios);
    }


    ///<summary>
    /// Atualizar usuario
    /// <summary>
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]

    public async Task<IActionResult> Atualizar(int id, [FromBody] CriarUsuarioDto dto)
    {
        _logger.LogInformation("Atualizando usuario com ID: {Id}}, id");

        var resultado = await _usuarioService.AtualizarAsync(id, dto);

        if(!resultado.Success)
        {
            if(resultado.Message.Contains("não encontrado"))
                return NotFound(resultado);

            return BadRequest(resultado);
        }

        return Ok(resultado);
    }



    ///<summary>
    /// Desativar usuario
    /// <summary>
    

    [HttpPut("{id}/desativar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Desativar(int id)
    {
        _logger.LogInformation("Desativando usuario com ID: {Id}", id);


        var resultado = await _usuarioService.DesativarAsync(id);

        if(!resultado.Success)
        {
            if(resultado.Message.Contains("não encontrado"))
                return NotFound(resultado);

            return BadRequest(resultado);
        }

        return Ok(resultado);
    }


    ///<summary>
    /// Criar usuario no sistema
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Criar([FromBody] CriarUsuarioDto dto)
    {
        _logger.LogInformation("Administrador tentando criar novo usuario: {Email}", dto.Email);

        var resultado = await _usuarioService.CriarUsuarioAsync(dto);

        if(!resultado.Success)
        {
            _logger.LogWarning("Falha ao criar usuario: {Message}", resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("Usuario criado com sucesso: {Email}", dto.Email);

        //retorna 201 created com localizacao do novo recurso
        return CreatedAtAction(nameof(ObterPorId), new {Id = resultado.Data}, resultado);
    }


    ///<summary>
    /// Atualizar role de usuario
    

    [HttpPut("{id}/roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]

    public async Task<IActionResult> AtualizarRoles(int id, [FromBody] RoleDto dto)
    {
        _logger.LogInformation("Atualizando roles do usuario ID: {Id}", id);

        var resultado = await _usuarioService.
    }
}
    



