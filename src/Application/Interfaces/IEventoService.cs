using Application.DTOs.Evento;
using Application.DTOs.Response;



public interface IEventoService
{
    Task<ResponseResult> CriarAsync(CriarEventoDto dto);

    Task<EventoDto?> ObterPorIdAsync(int id);

    Task<IEnumerable<EventoDto>> ObterTodosAsync();

    Task<ResponseResult> AtualizarAsync(int IdEvento, AtualizarEventoDTO dto);

    Task<ResponseResult> ExcluirAsync(int IdEvento);
}