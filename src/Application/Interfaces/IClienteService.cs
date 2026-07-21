using Application.DTOs.Cliente;
using Application.DTOs.Response;

namespace Application.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<ClienteDTO>> ObterTodosAsync();
    Task<IEnumerable<ClienteDTO>> ObterAtivosAsync();
    Task<ClienteDTO?> ObterPorIdAsync(int idCliente);
    Task<ResponseResult> CriarAsync(CriarClienteDTO dto);
    Task<ResponseResult> AtualizarAsync(int idCliente, AtualizarClienteDTO dto);
    Task<ResponseResult> DesativarAsync(int idCliente);
    Task<ResponseResult> AtivarAsync(int idCliente);
}