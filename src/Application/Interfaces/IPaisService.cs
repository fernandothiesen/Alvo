using Application.DTOs.Pais;
using Application.DTOs.Response;
public interface IPaisService
{
    Task<IEnumerable<PaisDTO>> ObterTodosAsync();
    Task<PaisDTO?> ObterPorIdAsync(int idPais);
    Task<ResponseResult> CriarAsync(CriarPaisDTO dto);
    Task<ResponseResult> AtualizarAsync(int idPais, AtualizarPaisDTO dto);
    Task<ResponseResult> ExcluirAsync(int idPais);
}