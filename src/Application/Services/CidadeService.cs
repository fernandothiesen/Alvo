using Application.DTOs.Cidade;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class CidadeService : ICidadeService
{
    private readonly ICidadeRepository _cidadeRepository;
    private readonly IEstadoRepository _estadoRepository;
    private readonly ILogger<CidadeService> _logger;

    public CidadeService(
        ICidadeRepository cidadeRepository, 
        IEstadoRepository estadoRepository,
        ILogger<CidadeService> logger)
    {
        _cidadeRepository = cidadeRepository;
        _estadoRepository = estadoRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<CidadeDTO>> ObterTodosAsync()
    {
        var cidades = await _cidadeRepository.ObterTodosAsync();
        return cidades.Select(MapearParaDTO);
    }

    public async Task<IEnumerable<CidadeDTO>> ObterPorEstadoAsync(int idEstado)
    {
        var cidades = await _cidadeRepository.ObterPorEstadoAsync(idEstado);
        return cidades.Select(MapearParaDTO);
    }

    public async Task<CidadeDTO?> ObterPorIdAsync(int idCidade)
    {
        var cidade = await _cidadeRepository.ObterPorIdAsync(idCidade);
        return cidade == null ? null : MapearParaDTO(cidade);
    }

    public async Task<ResponseResult> CriarAsync(CriarCidadeDTO dto)
    {
        try
        {
            // 1. Validação: Verificar se o estado existe
            var estado = await _estadoRepository.ObterPorIdAsync(dto.IdEstado);
            if (estado == null)
                return ResponseResult.Erro($"O estado com ID {dto.IdEstado} não existe.");

            // 2. Regra de Negócio: Nome único dentro do mesmo estado (opcional, mas recomendado)
            if (await _cidadeRepository.NomeExisteNoEstadoAsync(dto.IdEstado, dto.NomeCidade))
                return ResponseResult.Erro($"Já existe uma cidade com o nome '{dto.NomeCidade}' neste estado.");

            var cidade = new Cidade(dto.IdEstado, dto.NomeCidade);
            await _cidadeRepository.AdicionarAsync(cidade);

            _logger.LogInformation("Cidade {Nome} criada para o estado ID {IdEstado}", dto.NomeCidade, dto.IdEstado);
            return ResponseResult.Sucesso("Cidade criada com sucesso!", new { Id = cidade.IdCidade });
        }
        catch (DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar cidade {Nome}", dto.NomeCidade);
            return ResponseResult.Erro($"Erro ao criar cidade: {ex.Message}");
        }
    }

    public async Task<ResponseResult> AtualizarAsync(int idCidade, AtualizarCidadeDTO dto)
    {
        try
        {
            var cidade = await _cidadeRepository.ObterPorIdAsync(idCidade);
            if (cidade == null)
                return ResponseResult.Erro("Cidade não encontrada.");

            cidade.Atualizar(dto.NomeCidade);
            await _cidadeRepository.AtualizarAsync(cidade);

            _logger.LogInformation("Cidade ID {Id} atualizada com sucesso", idCidade);
            return ResponseResult.Sucesso("Cidade atualizada com sucesso!");
        }
        catch (DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar cidade ID {Id}", idCidade);
            return ResponseResult.Erro($"Erro ao atualizar cidade: {ex.Message}");
        }
    }

    public async Task<ResponseResult> ExcluirAsync(int idCidade)
    {
        try
        {
            var cidade = await _cidadeRepository.ObterPorIdAsync(idCidade);
            if (cidade == null)
                return ResponseResult.Erro("Cidade não encontrada.");

            await _cidadeRepository.RemoverAsync(idCidade);

            _logger.LogInformation("Cidade ID {Id} excluída com sucesso", idCidade);
            return ResponseResult.Sucesso("Cidade excluída com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir cidade ID {Id}", idCidade);
            return ResponseResult.Erro($"Erro ao excluir cidade: {ex.Message}");
        }
    }

    #region Métodos Auxiliares

    private CidadeDTO MapearParaDTO(Cidade cidade)
    {
        return new CidadeDTO
        {
            IdCidade = cidade.IdCidade,
            IdEstado = cidade.IdEstado,
            NomeEstado = cidade.Estado?.NomeEstado ?? "Desconhecido",
            SiglaEstado = cidade.Estado?.SiglaEstado ?? "--",
            NomeCidade = cidade.NomeCidade
        };
    }

    #endregion
}