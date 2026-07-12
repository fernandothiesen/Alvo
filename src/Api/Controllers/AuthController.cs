using Application.DTOs.Auth;
using Application.DTOs.Usuario;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUsuarioService _usuarioService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, IUsuarioService usuarioService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _usuarioService = usuarioService;
        _logger = logger;
    }

    ///<summary>
    /// Realiza login no sistema
    /// <summary>
    /// <param name="dto">Credenciais do usuário</param>
    /// <returns>Token JWT e dados do usuário</returns>
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        _logger.LogInformation("Tentativa de login para o email: {Email}", loginDto.Email);

        var resultado = await _authService.LoginAsync(loginDto);

        if(!resultado.Success)
        {
            _logger.LogWarning("Falha no login para o email: {Email} - {Message}", loginDto.Email, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("Login realizado com sucesso para o email: {Email}", loginDto.Email);
        return Ok(resultado);
    }
}