using Application.DTOs.Estado;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class EstadoService : IEstadoService
{
    private readonly IEstadoRepository _estadoRepository;
    private readonly IPaisRepository _paisRepository;
    private readonly ILogger<EstadoService> _logger;

    public EstadoService(
        IEstadoRepository estadoRepository, 
        IPaisRepository paisRepository,
        ILogger<EstadoService> logger)
    {
        _estadoRepository = estadoRepository;
        _paisRepository = paisRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<EstadoDTO>> ObterTodosAsync()
    {
        var estados = await _estadoRepository.ObterTodosAsync();
        return estados.Select(MapearParaDTO);
    }

    public async Task<IEnumerable<EstadoDTO>> ObterPorPaisAsync(int idPais)
    {
        var estados = await _estadoRepository.ObterPorPaisAsync(idPais);
        return estados.Select(MapearParaDTO);
    }

    public async Task<EstadoDTO?> ObterPorIdAsync(int idEstado)
    {
        var estado = await _estadoRepository.ObterPorIdAsync(idEstado);
        return estado == null ? null : MapearParaDTO(estado);
    }

    public async Task<ResponseResult> CriarAsync(CriarEstadoDTO dto)
    {
        try
        {
            // 1. Validação: Verificar se o país existe
            var pais = await _paisRepository.ObterPorIdAsync(dto.IdPais);
            if (pais == null)
                return ResponseResult.Erro($"O país com ID {dto.IdPais} não existe.");

            // 2. Regra de Negócio: Sigla única dentro do mesmo país
            if (await _estadoRepository.SiglaExisteNoPaisAsync(dto.IdPais, dto.SiglaEstado))
                return ResponseResult.Erro($"Já existe um estado com a sigla '{dto.SiglaEstado}' neste país.");

            var estado = new Estado(dto.IdPais, dto.NomeEstado, dto.SiglaEstado);
            await _estadoRepository.AdicionarAsync(estado);

            _logger.LogInformation("Estado {Nome} ({Sigla}) criado para o país ID {IdPais}", dto.NomeEstado, dto.SiglaEstado, dto.IdPais);
            return ResponseResult.Sucesso("Estado criado com sucesso!", new { Id = estado.IdEstado });
        }
        catch (DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar estado {Nome}", dto.NomeEstado);
            return ResponseResult.Erro($"Erro ao criar estado: {ex.Message}");
        }
    }

    public async Task<ResponseResult> AtualizarAsync(int idEstado, AtualizarEstadoDTO dto)
    {
        try
        {
            var estado = await _estadoRepository.ObterPorIdAsync(idEstado);
            if (estado == null)
                return ResponseResult.Erro("Estado não encontrado.");

            // Validação de sigla única (excluindo o próprio estado)
            if (await _estadoRepository.SiglaExisteNoPaisAsync(estado.IdPais, dto.SiglaEstado, idEstado))
                return ResponseResult.Erro($"Já existe outro estado com a sigla '{dto.SiglaEstado}' neste país.");

            estado.Atualizar(dto.NomeEstado, dto.SiglaEstado);
            await _estadoRepository.AtualizarAsync(estado);

            _logger.LogInformation("Estado ID {Id} atualizado com sucesso", idEstado);
            return ResponseResult.Sucesso("Estado atualizado com sucesso!");
        }
        catch (DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar estado ID {Id}", idEstado);
            return ResponseResult.Erro($"Erro ao atualizar estado: {ex.Message}");
        }
    }

    public async Task<ResponseResult> ExcluirAsync(int idEstado)
    {
        try
        {
            var estado = await _estadoRepository.ObterPorIdAsync(idEstado);
            if (estado == null)
                return ResponseResult.Erro("Estado não encontrado.");

            await _estadoRepository.RemoverAsync(idEstado);

            _logger.LogInformation("Estado ID {Id} excluído com sucesso", idEstado);
            return ResponseResult.Sucesso("Estado excluído com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir estado ID {Id}", idEstado);
            return ResponseResult.Erro($"Erro ao excluir estado: {ex.Message}");
        }
    }

    #region Métodos Auxiliares

    private EstadoDTO MapearParaDTO(Estado estado)
    {
        return new EstadoDTO
        {
            IdEstado = estado.IdEstado,
            IdPais = estado.IdPais,
            NomePais = estado.Pais?.NomePais ?? "Desconhecido",
            NomeEstado = estado.NomeEstado,
            SiglaEstado = estado.SiglaEstado
        };
    }

    #endregion
}