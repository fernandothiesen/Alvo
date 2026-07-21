using Application.DTOs.Fornecedor;
using Application.DTOs.Response;

namespace Application.Interfaces;

public interface IFornecedorService
{
    Task<IEnumerable<FornecedorDTO>> ObterTodosAsync();
    Task<IEnumerable<FornecedorDTO>> ObterAtivosAsync();
    Task<FornecedorDTO?> ObterPorIdAsync(int idFornecedor);
    Task<ResponseResult> CriarAsync(CriarFornecedorDTO dto);
    Task<ResponseResult> AtualizarAsync(int idFornecedor, AtualizarFornecedorDTO dto);
    Task<ResponseResult> DesativarAsync(int idFornecedor);
    Task<ResponseResult> AtivarAsync(int idFornecedor);
}