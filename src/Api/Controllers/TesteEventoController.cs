using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/teste")]
[Authorize]
public class TesteController : ControllerBase
{
    private readonly IEventoRepository _eventoRepository;

    public TesteController(IEventoRepository eventoRepository)
    {
        _eventoRepository = eventoRepository;
    }

    [HttpGet("eventos")]
    public async Task<IActionResult> TestarEventoRepository()
    {
        var eventos = await _eventoRepository.ObterTodosAsync();
        return Ok(eventos);
    }

    [HttpGet("eventos/{id}")]
    public async Task<IActionResult> TestarEventoCompleto(int id)
    {
        var evento = await _eventoRepository.ObterPorIdCompletoAsync(id);
        return Ok(evento);
    }
}