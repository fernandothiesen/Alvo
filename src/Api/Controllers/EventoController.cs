using Application.DTOs.Evento;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requer autenticação em TODOS os endpoints
public class EventosController : ControllerBase
{
    private readonly IEventoService _eventoService;
    private readonly ILogger<EventosController> _logger;

    public EventosController(
        IEventoService eventoService,
        ILogger<EventosController> logger)
    {
        _eventoService = eventoService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os eventos do sistema
    /// </summary>
    /// <remarks>
    /// Requer permissão: VisualizarEventos
    /// Retorna eventos ordenados por data de início (mais recentes primeiro)
    /// </remarks>
    [HttpGet]
    [Authorize(Policy = "VisualizarEventos")]
    [ProducesResponseType(typeof(IEnumerable<EventoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObterTodos()
    {
        _logger.LogInformation("Listando todos os eventos - Usuário: {User}", User.Identity?.Name);

        var eventos = await _eventoService.ObterTodosAsync();

        return Ok(eventos);
    }

    /// <summary>
    /// Obtém um evento específico pelo ID
    /// </summary>
    /// <param name="id">ID do evento</param>
    /// <remarks>
    /// Requer permissão: VisualizarEventos
    /// Retorna o evento com todos os relacionamentos (cidade, responsável, clientes, fornecedores)
    /// </remarks>
    [HttpGet("{id}")]
    [Authorize(Policy = "VisualizarEventos")]
    [ProducesResponseType(typeof(EventoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        _logger.LogInformation("Buscando evento ID: {Id} - Usuário: {User}", id, User.Identity?.Name);

        var evento = await _eventoService.ObterPorIdAsync(id);

        if (evento == null)
        {
            _logger.LogWarning("Evento ID {Id} não encontrado", id);
            return NotFound(new { message = "Evento não encontrado" });
        }

        return Ok(evento);
    }

    /// <summary>
    /// Cria um novo evento
    /// </summary>
    /// <param name="dto">Dados do evento a ser criado</param>
    /// <remarks>
    /// Requer permissão: GerenciarEventos
    /// O código do evento deve ser único no sistema
    /// </remarks>
    [HttpPost]
    [Authorize(Policy = "GerenciarEventos")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Criar([FromBody] CriarEventoDto dto)
    {
        _logger.LogInformation("Criando evento: {Codigo} - Usuário: {User}", 
            dto.CodigoEvento, User.Identity?.Name);

        var resultado = await _eventoService.CriarAsync(dto);

        if (!resultado.Success)
        {
            _logger.LogWarning("Falha ao criar evento {Codigo}: {Message}", 
                dto.CodigoEvento, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("Evento {Codigo} criado com sucesso - ID: {Id}", 
            dto.CodigoEvento, resultado.Data);

        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data }, resultado);
    }

    /// <summary>
    /// Atualiza um evento existente
    /// </summary>
    /// <param name="id">ID do evento a ser atualizado</param>
    /// <param name="dto">Dados atualizados do evento</param>
    /// <remarks>
    /// Requer permissão: GerenciarEventos
    /// O código do evento não pode ser alterado
    /// </remarks>
    [HttpPut("{id}")]
    [Authorize(Policy = "GerenciarEventos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarEventoDTO dto)
    {
        _logger.LogInformation("Atualizando evento ID: {Id} - Usuário: {User}", 
            id, User.Identity?.Name);

        var resultado = await _eventoService.AtualizarAsync(id, dto);

        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrado"))
            {
                _logger.LogWarning("Evento ID {Id} não encontrado para atualização", id);
                return NotFound(resultado);
            }

            _logger.LogWarning("Falha ao atualizar evento ID {Id}: {Message}", 
                id, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("Evento ID {Id} atualizado com sucesso", id);
        return Ok(resultado);
    }

    /// <summary>
    /// Exclui um evento do sistema
    /// </summary>
    /// <param name="id">ID do evento a ser excluído</param>
  
    [HttpDelete("{id}")]
    [Authorize(Policy = "GerenciarEventos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Excluir(int id)
    {
        _logger.LogWarning(" Tentativa de exclusão do evento ID: {Id} - Usuário: {User}", 
            id, User.Identity?.Name);

        var resultado = await _eventoService.ExcluirAsync(id);

        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrado"))
            {
                _logger.LogWarning("Evento ID {Id} não encontrado para exclusão", id);
                return NotFound(resultado);
            }

            _logger.LogError("Erro ao excluir evento ID {Id}: {Message}", 
                id, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("Evento ID {Id} excluído com sucesso", id);
        return Ok(resultado);
    }
}