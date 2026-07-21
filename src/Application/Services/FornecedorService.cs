using Application.DTOs.Fornecedor;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class FornecedorService : IFornecedorService
{
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly ICidadeRepository _cidadeRepository;
    private readonly ILogger<FornecedorService> _logger;

    public FornecedorService(
        IFornecedorRepository fornecedorRepository, 
        ICidadeRepository cidadeRepository,
        ILogger<FornecedorService> logger)
    {
        _fornecedorRepository = fornecedorRepository;
        _cidadeRepository = cidadeRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<FornecedorDTO>> ObterTodosAsync()
    {
        var fornecedores = await _fornecedorRepository.ObterTodosAsync();
        return fornecedores.Select(MapearParaDTO);
    }

    public async Task<IEnumerable<FornecedorDTO>> ObterAtivosAsync()
    {
        var fornecedores = await _fornecedorRepository.ObterAtivosAsync();
        return fornecedores.Select(MapearParaDTO);
    }

    public async Task<FornecedorDTO?> ObterPorIdAsync(int idFornecedor)
    {
        var fornecedor = await _fornecedorRepository.ObterPorIdAsync(idFornecedor);
        return fornecedor == null ? null : MapearParaDTO(fornecedor);
    }

    public async Task<ResponseResult> CriarAsync(CriarFornecedorDTO dto)
    {
        try
        {
            // 1. Regra de Negócio: Nome único
            if (await _fornecedorRepository.NomeExisteAsync(dto.Nome))
                return ResponseResult.Erro($"Já existe um fornecedor com o nome '{dto.Nome}'.");

            // 2. Validação: Verificar se a cidade existe (se fornecida)
            if (dto.IdCidade.HasValue)
            {
                var cidade = await _cidadeRepository.ObterPorIdAsync(dto.IdCidade.Value);
                if (cidade == null)
                    return ResponseResult.Erro($"A cidade com ID {dto.IdCidade} não existe.");
            }

            // 3. Criar a Entidade de Domínio
            var fornecedor = new Fornecedor(dto.Nome, dto.IdCidade);
            await _fornecedorRepository.AdicionarAsync(fornecedor);

            _logger.LogInformation("Fornecedor {Nome} criado com sucesso - ID: {Id}", dto.Nome, fornecedor.IdFornecedor);
            return ResponseResult.Sucesso("Fornecedor criado com sucesso!", new { Id = fornecedor.IdFornecedor });
        }
        catch (DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar fornecedor {Nome}", dto.Nome);
            return ResponseResult.Erro($"Erro ao criar fornecedor: {ex.Message}");
        }
    }

    public async Task<ResponseResult> AtualizarAsync(int idFornecedor, AtualizarFornecedorDTO dto)
    {
        try
        {
            var fornecedor = await _fornecedorRepository.ObterPorIdAsync(idFornecedor);
            if (fornecedor == null)
                return ResponseResult.Erro("Fornecedor não encontrado.");

            // Regra de Negócio: Nome único (excluindo o próprio fornecedor)
            if (await _fornecedorRepository.NomeExisteAsync(dto.Nome, idFornecedor))
                return ResponseResult.Erro($"Já existe outro fornecedor com o nome '{dto.Nome}'.");

            // Validação: Verificar se a cidade existe (se fornecida)
            if (dto.IdCidade.HasValue)
            {
                var cidade = await _cidadeRepository.ObterPorIdAsync(dto.IdCidade.Value);
                if (cidade == null)
                    return ResponseResult.Erro($"A cidade com ID {dto.IdCidade} não existe.");
            }

            fornecedor.Atualizar(dto.Nome, dto.IdCidade);
            await _fornecedorRepository.AtualizarAsync(fornecedor);

            _logger.LogInformation("Fornecedor ID {Id} atualizado com sucesso", idFornecedor);
            return ResponseResult.Sucesso("Fornecedor atualizado com sucesso!");
        }
        catch (DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar fornecedor ID {Id}", idFornecedor);
            return ResponseResult.Erro($"Erro ao atualizar fornecedor: {ex.Message}");
        }
    }

    public async Task<ResponseResult> DesativarAsync(int idFornecedor)
    {
        try
        {
            var fornecedor = await _fornecedorRepository.ObterPorIdAsync(idFornecedor);
            if (fornecedor == null)
                return ResponseResult.Erro("Fornecedor não encontrado.");

            fornecedor.Desativar();
            await _fornecedorRepository.AtualizarAsync(fornecedor);

            _logger.LogInformation("Fornecedor ID {Id} desativado com sucesso", idFornecedor);
            return ResponseResult.Sucesso("Fornecedor desativado com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao desativar fornecedor ID {Id}", idFornecedor);
            return ResponseResult.Erro($"Erro ao desativar fornecedor: {ex.Message}");
        }
    }

    public async Task<ResponseResult> AtivarAsync(int idFornecedor)
    {
        try
        {
            var fornecedor = await _fornecedorRepository.ObterPorIdAsync(idFornecedor);
            if (fornecedor == null)
                return ResponseResult.Erro("Fornecedor não encontrado.");

            fornecedor.Ativar();
            await _fornecedorRepository.AtualizarAsync(fornecedor);

            _logger.LogInformation("Fornecedor ID {Id} ativado com sucesso", idFornecedor);
            return ResponseResult.Sucesso("Fornecedor ativado com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ativar fornecedor ID {Id}", idFornecedor);
            return ResponseResult.Erro($"Erro ao ativar fornecedor: {ex.Message}");
        }
    }

    #region Métodos Auxiliares

    private FornecedorDTO MapearParaDTO(Fornecedor fornecedor)
    {
        return new FornecedorDTO
        {
            IdFornecedor = fornecedor.IdFornecedor,
            Nome = fornecedor.Nome,
            Ativo = fornecedor.Ativo,
            DataCadastro = fornecedor.DataCadastro,
            IdCidade = fornecedor.IdCidade,
            NomeCidade = fornecedor.Cidade?.NomeCidade,
            SiglaEstado = fornecedor.Cidade?.Estado?.SiglaEstado
        };
    }

    #endregion
}