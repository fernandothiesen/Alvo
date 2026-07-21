using Application.DTOs.Cliente;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requer autenticação em TODOS os endpoints
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;
    private readonly ILogger<ClientesController> _logger;

    public ClientesController(
        IClienteService clienteService,
        ILogger<ClientesController> logger)
    {
        _clienteService = clienteService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os clientes (ativos e inativos)
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(typeof(IEnumerable<ClienteDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObterTodos()
    {
        _logger.LogInformation("Listando todos os clientes - Usuário: {User}", User.Identity?.Name);

        var clientes = await _clienteService.ObterTodosAsync();
        return Ok(clientes);
    }

    /// <summary>
    /// Lista apenas clientes ativos
    /// </summary>
    [HttpGet("ativos")]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(typeof(IEnumerable<ClienteDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObterAtivos()
    {
        _logger.LogInformation("Listando clientes ativos - Usuário: {User}", User.Identity?.Name);

        var clientes = await _clienteService.ObterAtivosAsync();
        return Ok(clientes);
    }

    /// <summary>
    /// Obtém um cliente específico pelo ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(typeof(ClienteDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        _logger.LogInformation("Buscando cliente ID: {Id} - Usuário: {User}", id, User.Identity?.Name);

        var cliente = await _clienteService.ObterPorIdAsync(id);

        if (cliente == null)
        {
            _logger.LogWarning("Cliente ID {Id} não encontrado", id);
            return NotFound(new { message = "Cliente não encontrado" });
        }

        return Ok(cliente);
    }

    /// <summary>
    /// Cria um novo cliente
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Criar([FromBody] CriarClienteDTO dto)
    {
        _logger.LogInformation("Criando cliente: {Nome} - Usuário: {User}", dto.Nome, User.Identity?.Name);

        var resultado = await _clienteService.CriarAsync(dto);

        if (!resultado.Success)
        {
            _logger.LogWarning("Falha ao criar cliente {Nome}: {Message}", dto.Nome, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("Cliente {Nome} criado com sucesso - ID: {Id}", dto.Nome, resultado.Data);
        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data }, resultado);
    }

    /// <summary>
    /// Atualiza um cliente existente
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarClienteDTO dto)
    {
        _logger.LogInformation("Atualizando cliente ID: {Id} - Usuário: {User}", id, User.Identity?.Name);

        var resultado = await _clienteService.AtualizarAsync(id, dto);

        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrado"))
            {
                _logger.LogWarning("Cliente ID {Id} não encontrado para atualização", id);
                return NotFound(resultado);
            }

            _logger.LogWarning("Falha ao atualizar cliente ID {Id}: {Message}", id, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("Cliente ID {Id} atualizado com sucesso", id);
        return Ok(resultado);
    }

    /// <summary>
    /// Desativa um cliente (soft delete)
    /// </summary>
    [HttpPut("{id}/desativar")]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Desativar(int id)
    {
        _logger.LogInformation("Desativando cliente ID: {Id} - Usuário: {User}", id, User.Identity?.Name);

        var resultado = await _clienteService.DesativarAsync(id);

        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrado"))
            {
                _logger.LogWarning("Cliente ID {Id} não encontrado para desativação", id);
                return NotFound(resultado);
            }

            _logger.LogWarning("Falha ao desativar cliente ID {Id}: {Message}", id, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("Cliente ID {Id} desativado com sucesso", id);
        return Ok(resultado);
    }

    /// <summary>
    /// Ativa um cliente previamente desativado
    /// </summary>
    [HttpPut("{id}/ativar")]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Ativar(int id)
    {
        _logger.LogInformation("Ativando cliente ID: {Id} - Usuário: {User}", id, User.Identity?.Name);

        var resultado = await _clienteService.AtivarAsync(id);

        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrado"))
            {
                _logger.LogWarning("Cliente ID {Id} não encontrado para ativação", id);
                return NotFound(resultado);
            }

            _logger.LogWarning("Falha ao ativar cliente ID {Id}: {Message}", id, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("Cliente ID {Id} ativado com sucesso", id);
        return Ok(resultado);
    }
}