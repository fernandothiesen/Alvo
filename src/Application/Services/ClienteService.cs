using Application.DTOs.Cliente;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly ICidadeRepository _cidadeRepository;
    private readonly ILogger<ClienteService> _logger;

    public ClienteService(
        IClienteRepository clienteRepository, 
        ICidadeRepository cidadeRepository,
        ILogger<ClienteService> logger)
    {
        _clienteRepository = clienteRepository;
        _cidadeRepository = cidadeRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ClienteDTO>> ObterTodosAsync()
    {
        var clientes = await _clienteRepository.ObterTodosAsync();
        return clientes.Select(MapearParaDTO);
    }

    public async Task<IEnumerable<ClienteDTO>> ObterAtivosAsync()
    {
        var clientes = await _clienteRepository.ObterAtivosAsync();
        return clientes.Select(MapearParaDTO);
    }

    public async Task<ClienteDTO?> ObterPorIdAsync(int idCliente)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(idCliente);
        return cliente == null ? null : MapearParaDTO(cliente);
    }

    public async Task<ResponseResult> CriarAsync(CriarClienteDTO dto)
    {
        try
        {
            // 1. Regra de Negócio: Nome único
            if (await _clienteRepository.NomeExisteAsync(dto.Nome))
                return ResponseResult.Erro($"Já existe um cliente com o nome '{dto.Nome}'.");

            // 2. Validação: Verificar se a cidade existe (se fornecida)
            if (dto.IdCidade.HasValue)
            {
                var cidade = await _cidadeRepository.ObterPorIdAsync(dto.IdCidade.Value);
                if (cidade == null)
                    return ResponseResult.Erro($"A cidade com ID {dto.IdCidade} não existe.");
            }

            // 3. Criar a Entidade de Domínio
            var cliente = new Cliente(dto.Nome, dto.IdCidade);
            await _clienteRepository.AdicionarAsync(cliente);

            _logger.LogInformation("Cliente {Nome} criado com sucesso - ID: {Id}", dto.Nome, cliente.IdCliente);
            return ResponseResult.Sucesso("Cliente criado com sucesso!", new { Id = cliente.IdCliente });
        }
        catch (DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar cliente {Nome}", dto.Nome);
            return ResponseResult.Erro($"Erro ao criar cliente: {ex.Message}");
        }
    }

    public async Task<ResponseResult> AtualizarAsync(int idCliente, AtualizarClienteDTO dto)
    {
        try
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(idCliente);
            if (cliente == null)
                return ResponseResult.Erro("Cliente não encontrado.");

            // Regra de Negócio: Nome único (excluindo o próprio cliente)
            if (await _clienteRepository.NomeExisteAsync(dto.Nome, idCliente))
                return ResponseResult.Erro($"Já existe outro cliente com o nome '{dto.Nome}'.");

            // Validação: Verificar se a cidade existe (se fornecida)
            if (dto.IdCidade.HasValue)
            {
                var cidade = await _cidadeRepository.ObterPorIdAsync(dto.IdCidade.Value);
                if (cidade == null)
                    return ResponseResult.Erro($"A cidade com ID {dto.IdCidade} não existe.");
            }

            cliente.Atualizar(dto.Nome, dto.IdCidade);
            await _clienteRepository.AtualizarAsync(cliente);

            _logger.LogInformation("Cliente ID {Id} atualizado com sucesso", idCliente);
            return ResponseResult.Sucesso("Cliente atualizado com sucesso!");
        }
        catch (DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar cliente ID {Id}", idCliente);
            return ResponseResult.Erro($"Erro ao atualizar cliente: {ex.Message}");
        }
    }

    public async Task<ResponseResult> DesativarAsync(int idCliente)
    {
        try
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(idCliente);
            if (cliente == null)
                return ResponseResult.Erro("Cliente não encontrado.");

            cliente.Desativar();
            await _clienteRepository.AtualizarAsync(cliente);

            _logger.LogInformation("Cliente ID {Id} desativado com sucesso", idCliente);
            return ResponseResult.Sucesso("Cliente desativado com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao desativar cliente ID {Id}", idCliente);
            return ResponseResult.Erro($"Erro ao desativar cliente: {ex.Message}");
        }
    }

    public async Task<ResponseResult> AtivarAsync(int idCliente)
    {
        try
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(idCliente);
            if (cliente == null)
                return ResponseResult.Erro("Cliente não encontrado.");

            cliente.Ativar();
            await _clienteRepository.AtualizarAsync(cliente);

            _logger.LogInformation("Cliente ID {Id} ativado com sucesso", idCliente);
            return ResponseResult.Sucesso("Cliente ativado com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ativar cliente ID {Id}", idCliente);
            return ResponseResult.Erro($"Erro ao ativar cliente: {ex.Message}");
        }
    }

    #region Métodos Auxiliares

    private ClienteDTO MapearParaDTO(Cliente cliente)
    {
        return new ClienteDTO
        {
            IdCliente = cliente.IdCliente,
            Nome = cliente.Nome,
            Ativo = cliente.Ativo,
            DataCadastro = cliente.DataCadastro,
            IdCidade = cliente.IdCidade,
            NomeCidade = cliente.Cidade?.NomeCidade,
            SiglaEstado = cliente.Cidade?.Estado?.SiglaEstado
        };
    }

    #endregion
}