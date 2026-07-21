using Application.DTOs.Estado;
using Application.DTOs.Response;


namespace Application.Interfaces;

public interface IEstadoService
{
    
    Task<IEnumerable<EstadoDTO>> ObterTodosAsync();
    Task<IEnumerable<EstadoDTO>> ObterPorPaisAsync(int idPais);
    Task<EstadoDTO?> ObterPorIdAsync(int idEstado);
    Task<ResponseResult> CriarAsync(CriarEstadoDTO dto);
    Task<ResponseResult> AtualizarAsync(int idEstado, AtualizarEstadoDTO dto);
    Task<ResponseResult> ExcluirAsync(int idEstado);

}