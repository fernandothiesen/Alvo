using Application.DTOs.Demanda;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DemandasController : ControllerBase
{
    private readonly IDemandaService _demandaService;
    private readonly ILogger<DemandasController> _logger;

    public DemandasController(IDemandaService demandaService, ILogger<DemandasController> logger)
    {
        _demandaService = demandaService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Policy = "GerenciarDemandas")]
    public async Task<IActionResult> ObterTodos()
    {
        var demandas = await _demandaService.ObterTodosAsync();
        return Ok(demandas);
    }

    [HttpGet("evento/{idEvento}")]
    [Authorize(Policy = "VisualizarEventos")] // Quem vê o evento, vê as demandas dele
    public async Task<IActionResult> ObterPorEvento(int idEvento)
    {
        var demandas = await _demandaService.ObterPorEventoAsync(idEvento);
        return Ok(demandas);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "GerenciarDemandas")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var demanda = await _demandaService.ObterPorIdAsync(id);
        if (demanda == null) return NotFound(new { message = "Demanda não encontrada" });
        return Ok(demanda);
    }

    [HttpPost]
    [Authorize(Policy = "GerenciarDemandas")]
    public async Task<IActionResult> Criar([FromBody] CriarDemandaDTO dto)
    {
        var resultado = await _demandaService.CriarAsync(dto);
        if (!resultado.Success) return BadRequest(resultado);

        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data }, resultado);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "GerenciarDemandas")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarDemandaDTO dto)
    {
        var resultado = await _demandaService.AtualizarAsync(id, dto);
        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrada")) return NotFound(resultado);
            return BadRequest(resultado);
        }
        return Ok(resultado);
    }

    [HttpPut("{id}/concluir")]
    [Authorize(Policy = "GerenciarDemandas")]
    public async Task<IActionResult> Concluir(int id)
    {
        var resultado = await _demandaService.ConcluirAsync(id);
        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrada")) return NotFound(resultado);
            return BadRequest(resultado);
        }
        return Ok(resultado);
    }
}