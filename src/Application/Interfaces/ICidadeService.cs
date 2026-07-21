using Application.DTOs.Cidade;
using Application.DTOs.Response;

namespace Application.Interfaces;

public interface ICidadeService
{
    Task<IEnumerable<CidadeDTO>> ObterTodosAsync();
    Task<IEnumerable<CidadeDTO>> ObterPorEstadoAsync(int idEstado);
    Task<CidadeDTO?> ObterPorIdAsync(int idCidade);
    Task<ResponseResult> CriarAsync(CriarCidadeDTO dto);
    Task<ResponseResult> AtualizarAsync(int idCidade, AtualizarCidadeDTO dto);
    Task<ResponseResult> ExcluirAsync(int idCidade);
}