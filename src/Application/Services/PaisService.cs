using Application.DTOs.Pais;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class PaisService : IPaisService
{
    private readonly IPaisRepository _paisRepository;
    private readonly ILogger<PaisService> _logger;

    public PaisService(IPaisRepository paisRepository, ILogger<PaisService> logger)
    {
        _paisRepository = paisRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<PaisDTO>> ObterTodosAsync()
    {
        var paises = await _paisRepository.ObterTodosAsync();
        return paises.Select(MapearParaDTO);
    }

    public async Task<PaisDTO?> ObterPorIdAsync(int idPais)
    {
        var pais = await _paisRepository.ObterPorIdAsync(idPais);
        return pais == null ? null : MapearParaDTO(pais);
    }

    public async Task<ResponseResult> CriarAsync(CriarPaisDTO dto)
    {
        try
        {
            // Regra de Negócio: Código ISO único
            if (await _paisRepository.CodigoIsoExisteAsync(dto.CodigoIso))
                return ResponseResult.Erro($"Já existe um país com o código ISO '{dto.CodigoIso}'.");

            var pais = new Pais(dto.NomePais, dto.CodigoIso);
            await _paisRepository.AdicionarAsync(pais);

            _logger.LogInformation("País {Nome} ({Iso}) criado com sucesso", dto.NomePais, dto.CodigoIso);
            return ResponseResult.Sucesso("País criado com sucesso!", new { Id = pais.IdPais });
        }
        catch (DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar país {Nome}", dto.NomePais);
            return ResponseResult.Erro($"Erro ao criar país: {ex.Message}");
        }
    }

    public async Task<ResponseResult> AtualizarAsync(int idPais, AtualizarPaisDTO dto)
    {
        try
        {
            var pais = await _paisRepository.ObterPorIdAsync(idPais);
            if (pais == null)
                return ResponseResult.Erro("País não encontrado.");

            // Regra de Negócio: Código ISO único (excluindo o próprio país)
            if (await _paisRepository.CodigoIsoExisteAsync(dto.CodigoIso, idPais))
                return ResponseResult.Erro($"Já existe outro país com o código ISO '{dto.CodigoIso}'.");

            pais.Atualizar(dto.NomePais, dto.CodigoIso);
            await _paisRepository.AtualizarAsync(pais);

            _logger.LogInformation("País ID {Id} atualizado com sucesso", idPais);
            return ResponseResult.Sucesso("País atualizado com sucesso!");
        }
        catch (DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar país ID {Id}", idPais);
            return ResponseResult.Erro($"Erro ao atualizar país: {ex.Message}");
        }
    }

    public async Task<ResponseResult> ExcluirAsync(int idPais)
    {
        try
        {
            var pais = await _paisRepository.ObterPorIdAsync(idPais);
            if (pais == null)
                return ResponseResult.Erro("País não encontrado.");

            await _paisRepository.RemoverAsync(idPais);

            _logger.LogInformation("País ID {Id} excluído com sucesso", idPais);
            return ResponseResult.Sucesso("País excluído com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir país ID {Id}", idPais);
            return ResponseResult.Erro($"Erro ao excluir país: {ex.Message}");
        }
    }

    #region Métodos Auxiliares

    private PaisDTO MapearParaDTO(Pais pais)
    {
        return new PaisDTO
        {
            IdPais = pais.IdPais,
            NomePais = pais.NomePais,
            CodigoIso = pais.CodigoIso
        };
    }

    #endregion
}