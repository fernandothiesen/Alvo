using Application.DTOs.Estado;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requer autenticação em TODOS os endpoints
public class EstadosController : ControllerBase
{
    private readonly IEstadoService _estadoService;
    private readonly ILogger<EstadosController> _logger;

    public EstadosController(
        IEstadoService estadoService,
        ILogger<EstadosController> logger)
    {
        _estadoService = estadoService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os estados cadastrados
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EstadoDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterTodos()
    {
        var estados = await _estadoService.ObterTodosAsync();
        return Ok(estados);
    }

    /// <summary>
    /// Lista estados filtrados por País
    /// </summary>
    /// <param name="idPais">ID do país</param>
    [HttpGet("por-pais/{idPais}")]
    [ProducesResponseType(typeof(IEnumerable<EstadoDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterPorPais(int idPais)
    {
        var estados = await _estadoService.ObterPorPaisAsync(idPais);
        return Ok(estados);
    }

    /// <summary>
    /// Obtém um estado específico pelo ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EstadoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var estado = await _estadoService.ObterPorIdAsync(id);
        if (estado == null)
            return NotFound(new { message = "Estado não encontrado" });

        return Ok(estado);
    }

    /// <summary>
    /// Cria um novo estado
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "GerenciarClientes")] // Usando a mesma política de cadastros básicos
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Criar([FromBody] CriarEstadoDTO dto)
    {
        var resultado = await _estadoService.CriarAsync(dto);
        if (!resultado.Success)
            return BadRequest(resultado);

        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data }, resultado);
    }

    /// <summary>
    /// Atualiza um estado existente
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarEstadoDTO dto)
    {
        var resultado = await _estadoService.AtualizarAsync(id, dto);
        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrado"))
                return NotFound(resultado);
            return BadRequest(resultado);
        }

        return Ok(resultado);
    }

    /// <summary>
    /// Exclui um estado
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Excluir(int id)
    {
        var resultado = await _estadoService.ExcluirAsync(id);
        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrado"))
                return NotFound(resultado);
            return BadRequest(resultado);
        }

        return Ok(resultado);
    }
}