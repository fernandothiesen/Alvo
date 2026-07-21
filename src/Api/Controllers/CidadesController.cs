using Application.DTOs.Cidade;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CidadesController : ControllerBase
{
    private readonly ICidadeService _cidadeService;
    private readonly ILogger<CidadesController> _logger;

    public CidadesController(ICidadeService cidadeService, ILogger<CidadesController> logger)
    {
        _cidadeService = cidadeService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CidadeDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterTodos()
    {
        var cidades = await _cidadeService.ObterTodosAsync();
        return Ok(cidades);
    }

    [HttpGet("por-estado/{idEstado}")]
    [ProducesResponseType(typeof(IEnumerable<CidadeDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorEstado(int idEstado)
    {
        var cidades = await _cidadeService.ObterPorEstadoAsync(idEstado);
        return Ok(cidades);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CidadeDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var cidade = await _cidadeService.ObterPorIdAsync(id);
        if (cidade == null) return NotFound(new { message = "Cidade não encontrada" });
        return Ok(cidade);
    }

    [HttpPost]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarCidadeDTO dto)
    {
        var resultado = await _cidadeService.CriarAsync(dto);
        if (!resultado.Success) return BadRequest(resultado);

        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data }, resultado);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarCidadeDTO dto)
    {
        var resultado = await _cidadeService.AtualizarAsync(id, dto);
        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrada")) return NotFound(resultado);
            return BadRequest(resultado);
        }
        return Ok(resultado);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(int id)
    {
        var resultado = await _cidadeService.ExcluirAsync(id);
        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrada")) return NotFound(resultado);
            return BadRequest(resultado);
        }
        return Ok(resultado);
    }
}