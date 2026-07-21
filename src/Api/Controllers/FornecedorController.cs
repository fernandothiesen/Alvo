using Application.DTOs.Fornecedor;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requer autenticação em TODOS os endpoints
public class FornecedoresController : ControllerBase
{
    private readonly IFornecedorService _fornecedorService;
    private readonly ILogger<FornecedoresController> _logger;

    public FornecedoresController(
        IFornecedorService fornecedorService,
        ILogger<FornecedoresController> logger)
    {
        _fornecedorService = fornecedorService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os fornecedores (ativos e inativos)
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "GerenciarFornecedores")]
    [ProducesResponseType(typeof(IEnumerable<FornecedorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObterTodos()
    {
        _logger.LogInformation("Listando todos os fornecedores - Usuário: {User}", User.Identity?.Name);

        var fornecedores = await _fornecedorService.ObterTodosAsync();
        return Ok(fornecedores);
    }

    /// <summary>
    /// Lista apenas fornecedores ativos
    /// </summary>
    [HttpGet("ativos")]
    [Authorize(Policy = "GerenciarFornecedores")]
    [ProducesResponseType(typeof(IEnumerable<FornecedorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObterAtivos()
    {
        _logger.LogInformation("Listando fornecedores ativos - Usuário: {User}", User.Identity?.Name);

        var fornecedores = await _fornecedorService.ObterAtivosAsync();
        return Ok(fornecedores);
    }

    /// <summary>
    /// Obtém um fornecedor específico pelo ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = "GerenciarFornecedores")]
    [ProducesResponseType(typeof(FornecedorDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        _logger.LogInformation("Buscando fornecedor ID: {Id} - Usuário: {User}", id, User.Identity?.Name);

        var fornecedor = await _fornecedorService.ObterPorIdAsync(id);

        if (fornecedor == null)
        {
            _logger.LogWarning("Fornecedor ID {Id} não encontrado", id);
            return NotFound(new { message = "Fornecedor não encontrado" });
        }

        return Ok(fornecedor);
    }

    /// <summary>
    /// Cria um novo fornecedor
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "GerenciarFornecedores")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Criar([FromBody] CriarFornecedorDTO dto)
    {
        _logger.LogInformation("Criando fornecedor: {Nome} - Usuário: {User}", dto.Nome, User.Identity?.Name);

        var resultado = await _fornecedorService.CriarAsync(dto);

        if (!resultado.Success)
        {
            _logger.LogWarning("Falha ao criar fornecedor {Nome}: {Message}", dto.Nome, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("Fornecedor {Nome} criado com sucesso - ID: {Id}", dto.Nome, resultado.Data);
        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data }, resultado);
    }

    /// <summary>
    /// Atualiza um fornecedor existente
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "GerenciarFornecedores")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarFornecedorDTO dto)
    {
        _logger.LogInformation("Atualizando fornecedor ID: {Id} - Usuário: {User}", id, User.Identity?.Name);

        var resultado = await _fornecedorService.AtualizarAsync(id, dto);

        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrado"))
            {
                _logger.LogWarning("Fornecedor ID {Id} não encontrado para atualização", id);
                return NotFound(resultado);
            }

            _logger.LogWarning("Falha ao atualizar fornecedor ID {Id}: {Message}", id, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("Fornecedor ID {Id} atualizado com sucesso", id);
        return Ok(resultado);
    }

    /// <summary>
    /// Desativa um fornecedor (soft delete)
    /// </summary>
    [HttpPut("{id}/desativar")]
    [Authorize(Policy = "GerenciarFornecedores")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Desativar(int id)
    {
        _logger.LogInformation("Desativando fornecedor ID: {Id} - Usuário: {User}", id, User.Identity?.Name);

        var resultado = await _fornecedorService.DesativarAsync(id);

        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrado"))
            {
                _logger.LogWarning("Fornecedor ID {Id} não encontrado para desativação", id);
                return NotFound(resultado);
            }

            _logger.LogWarning("Falha ao desativar fornecedor ID {Id}: {Message}", id, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("Fornecedor ID {Id} desativado com sucesso", id);
        return Ok(resultado);
    }

    /// <summary>
    /// Ativa um fornecedor previamente desativado
    /// </summary>
    [HttpPut("{id}/ativar")]
    [Authorize(Policy = "GerenciarFornecedores")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Ativar(int id)
    {
        _logger.LogInformation("Ativando fornecedor ID: {Id} - Usuário: {User}", id, User.Identity?.Name);

        var resultado = await _fornecedorService.AtivarAsync(id);

        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrado"))
            {
                _logger.LogWarning("Fornecedor ID {Id} não encontrado para ativação", id);
                return NotFound(resultado);
            }

            _logger.LogWarning("Falha ao ativar fornecedor ID {Id}: {Message}", id, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("Fornecedor ID {Id} ativado com sucesso", id);
        return Ok(resultado);
    }
}