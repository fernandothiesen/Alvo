using Application.DTOs.Demanda;
using Application.DTOs.Response;

namespace Application.Interfaces;

public interface IDemandaService
{
    Task<IEnumerable<DemandaDTO>> ObterTodosAsync();
    Task<IEnumerable<DemandaDTO>> ObterPorEventoAsync(int idEvento);
    Task<DemandaDTO?> ObterPorIdAsync(int idDemanda);
    Task<ResponseResult> CriarAsync(CriarDemandaDTO dto);
    Task<ResponseResult> AtualizarAsync(int idDemanda, AtualizarDemandaDTO dto);
    Task<ResponseResult> ConcluirAsync(int idDemanda);
}