using Application.DTOs.Pais;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requer autenticação em TODOS os endpoints
public class PaisesController : ControllerBase
{
    private readonly IPaisService _paisService;
    private readonly ILogger<PaisesController> _logger;

    public PaisesController(
        IPaisService paisService,
        ILogger<PaisesController> logger)
    {
        _paisService = paisService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os países cadastrados
    /// </summary>
    /// <remarks>
    /// Qualquer usuário autenticado pode consultar a lista de países.
    /// Retorna ordenado por nome.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PaisDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterTodos()
    {
        _logger.LogInformation("Listando todos os países - Usuário: {User}", User.Identity?.Name);

        var paises = await _paisService.ObterTodosAsync();
        return Ok(paises);
    }

    /// <summary>
    /// Obtém um país específico pelo ID
    /// </summary>
    /// <param name="id">ID do país</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PaisDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        _logger.LogInformation("Buscando país ID: {Id} - Usuário: {User}", id, User.Identity?.Name);

        var pais = await _paisService.ObterPorIdAsync(id);

        if (pais == null)
        {
            _logger.LogWarning("País ID {Id} não encontrado", id);
            return NotFound(new { message = "País não encontrado" });
        }

        return Ok(pais);
    }

    /// <summary>
    /// Cria um novo país
    /// </summary>
    /// <param name="dto">Dados do país a ser criado</param>
    /// <remarks>
    /// Requer permissão: GerenciarClientes
    /// O código ISO deve ser único e ter exatamente 3 caracteres (ex: BRA, USA, ARG)
    /// </remarks>
    [HttpPost]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Criar([FromBody] CriarPaisDTO dto)
    {
        _logger.LogInformation("Criando país: {Nome} ({Iso}) - Usuário: {User}",
            dto.NomePais, dto.CodigoIso, User.Identity?.Name);

        var resultado = await _paisService.CriarAsync(dto);

        if (!resultado.Success)
        {
            _logger.LogWarning("Falha ao criar país {Nome}: {Message}", dto.NomePais, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("País {Nome} criado com sucesso - ID: {Id}", dto.NomePais, resultado.Data);
        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data }, resultado);
    }

    /// <summary>
    /// Atualiza um país existente
    /// </summary>
    /// <param name="id">ID do país a ser atualizado</param>
    /// <param name="dto">Dados atualizados do país</param>
    /// <remarks>
    /// Requer permissão: GerenciarClientes
    /// </remarks>
    [HttpPut("{id}")]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarPaisDTO dto)
    {
        _logger.LogInformation("Atualizando país ID: {Id} - Usuário: {User}", id, User.Identity?.Name);

        var resultado = await _paisService.AtualizarAsync(id, dto);

        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrado"))
            {
                _logger.LogWarning("País ID {Id} não encontrado para atualização", id);
                return NotFound(resultado);
            }

            _logger.LogWarning("Falha ao atualizar país ID {Id}: {Message}", id, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("País ID {Id} atualizado com sucesso", id);
        return Ok(resultado);
    }

    /// <summary>
    /// Exclui um país do sistema
    /// </summary>
    /// <param name="id">ID do país a ser excluído</param>
    /// <remarks>
    /// Requer permissão: GerenciarClientes
    /// ️ Falhará se houver estados vinculados a este país (restrição de chave estrangeira)
    /// </remarks>
    [HttpDelete("{id}")]
    [Authorize(Policy = "GerenciarClientes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Excluir(int id)
    {
        _logger.LogWarning(" Tentativa de exclusão do país ID: {Id} - Usuário: {User}",
            id, User.Identity?.Name);

        var resultado = await _paisService.ExcluirAsync(id);

        if (!resultado.Success)
        {
            if (resultado.Message.Contains("não encontrado"))
            {
                _logger.LogWarning("País ID {Id} não encontrado para exclusão", id);
                return NotFound(resultado);
            }

            _logger.LogError("Erro ao excluir país ID {Id}: {Message}", id, resultado.Message);
            return BadRequest(resultado);
        }

        _logger.LogInformation("País ID {Id} excluído com sucesso", id);
        return Ok(resultado);
    }
}