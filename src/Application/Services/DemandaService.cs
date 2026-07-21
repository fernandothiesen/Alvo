using Application.DTOs.Demanda;
using Application.DTOs.Evento;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class DemandaService : IDemandaService
{
    private readonly IDemandaRepository _demandaRepository;
    private readonly IEventoRepository _eventoRepository;
    private readonly ILogger<DemandaService> _logger;

    public DemandaService(
        IDemandaRepository demandaRepository, 
        IEventoRepository eventoRepository,
        ILogger<DemandaService> logger)
    {
        _demandaRepository = demandaRepository;
        _eventoRepository = eventoRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<DemandaDTO>> ObterTodosAsync()
    {
        var demandas = await _demandaRepository.ObterTodosAsync();
        return demandas.Select(MapearParaDTO);
    }

    public async Task<IEnumerable<DemandaDTO>> ObterPorEventoAsync(int idEvento)
    {
        var demandas = await _demandaRepository.ObterPorEventoAsync(idEvento);
        return demandas.Select(MapearParaDTO);
    }

    public async Task<DemandaDTO?> ObterPorIdAsync(int idDemanda)
    {
        var demanda = await _demandaRepository.ObterPorIdCompletoAsync(idDemanda);
        return demanda == null ? null : MapearParaDTO(demanda);
    }

    public async Task<ResponseResult> CriarAsync(CriarDemandaDTO dto)
    {
        try
        {
            // 1. Validação CRÍTICA: O Evento deve existir
            var evento = await _eventoRepository.ObterPorIdAsync(dto.IdEvento);
            if (evento == null)
                return ResponseResult.Erro($"O evento com ID {dto.IdEvento} não existe.");

            // 2. Criar a Entidade de Domínio
            var demanda = new Demanda(dto.IdEvento, dto.Titulo, dto.Descricao, dto.Prioridade);

            // 3. Adicionar Relacionamentos N:N (Clientes e Fornecedores)
            if (dto.IdsClientes != null)
            {
                foreach (var idCliente in dto.IdsClientes)
                    demanda.AdicionarCliente(new DemandaCliente(demanda.IdDemanda, idCliente));
            }

            if (dto.IdsFornecedores != null)
            {
                foreach (var idFornecedor in dto.IdsFornecedores)
                    demanda.AdicionarFornecedor(new DemandaFornecedor(demanda.IdDemanda, idFornecedor));
            }

            await _demandaRepository.AdicionarAsync(demanda);

            _logger.LogInformation("Demanda '{Titulo}' criada para o Evento ID {IdEvento}", dto.Titulo, dto.IdEvento);
            return ResponseResult.Sucesso("Demanda criada com sucesso!", new { Id = demanda.IdDemanda });
        }
        catch (DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar demanda '{Titulo}'", dto.Titulo);
            return ResponseResult.Erro($"Erro ao criar demanda: {ex.Message}");
        }
    }

    public async Task<ResponseResult> AtualizarAsync(int idDemanda, AtualizarDemandaDTO dto)
    {
        try
        {
            var demanda = await _demandaRepository.ObterPorIdCompletoAsync(idDemanda);
            if (demanda == null)
                return ResponseResult.Erro("Demanda não encontrada.");

            demanda.Atualizar(dto.Titulo, dto.Descricao, dto.Prioridade, dto.Status);

            // Lógica de sincronização de N:N (Clientes e Fornecedores)
            // (Implementar método DefinirClientes/DefinirFornecedores no Domain se necessário, 
            // ou limpar e recriar as coleções via EF Core)
            
            await _demandaRepository.AtualizarAsync(demanda);
            return ResponseResult.Sucesso("Demanda atualizada com sucesso!");
        }
        catch (DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar demanda ID {Id}", idDemanda);
            return ResponseResult.Erro($"Erro ao atualizar demanda: {ex.Message}");
        }
    }

    public async Task<ResponseResult> ConcluirAsync(int idDemanda)
    {
        try
        {
            var demanda = await _demandaRepository.ObterPorIdAsync(idDemanda);
            if (demanda == null)
                return ResponseResult.Erro("Demanda não encontrada.");

            demanda.Concluir(); // Método do Domain que seta Status="Concluída" e DataConclusao=Now
            await _demandaRepository.AtualizarAsync(demanda);

            return ResponseResult.Sucesso("Demanda concluída com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao concluir demanda ID {Id}", idDemanda);
            return ResponseResult.Erro($"Erro ao concluir demanda: {ex.Message}");
        }
    }

    #region Métodos Auxiliares

    private DemandaDTO MapearParaDTO(Demanda demanda)
    {
        return new DemandaDTO
        {
            IdDemanda = demanda.IdDemanda,
            IdEvento = demanda.IdEvento,
            TituloEvento = demanda.Evento?.Titulo ?? "Evento não encontrado",
            Titulo = demanda.Titulo,
            Descricao = demanda.Descricao,
            Prioridade = demanda.Prioridade,
            Status = demanda.Status,
            DataCriacao = demanda.DataCriacao,
            DataConclusao = demanda.DataConclusao,
            
            Clientes = demanda.Clientes?.Select(c => new ItemResumoDto
            { 
                Id = c.IdCliente, 
                Nome = c.Cliente?.Nome ?? "Desconhecido" 
            }).ToList() ?? new List<ItemResumoDto>(),

            Fornecedores = demanda.Fornecedores?.Select(f => new ItemResumoDto 
            { 
                Id = f.IdFornecedor, 
                Nome = f.Fornecedor?.Nome ?? "Desconhecido" 
            }).ToList() ?? new List<ItemResumoDto>()
        };
    }

    #endregion
}