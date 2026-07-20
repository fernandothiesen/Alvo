using Application.DTOs.Evento;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class EventoService : IEventoService
{
    private readonly IEventoRepository _eventoRepository;
    private readonly ILogger<EventoService> _logger;

    public EventoService(IEventoRepository eventoRepository, ILogger<EventoService> logger)
    {
        _eventoRepository = eventoRepository;
        _logger = logger;
    }

    public async Task<ResponseResult> CriarAsync(CriarEventoDto dto)
    {
        try
        {
            // 1. Regra de Negócio: Código único
            if (await _eventoRepository.CodigoExisteAsync(dto.CodigoEvento))
                return ResponseResult.Erro("Já existe um evento com este código.");

            // 2. Criar a Entidade de Domínio
            var evento = new Evento(
                dto.CodigoEvento, 
                dto.Titulo, 
                dto.IdCidade, 
                dto.IdResponsavel
            );

            // Atualizar campos opcionais
            evento.Atualizar(
                dto.CodigoEvento, 
                dto.Titulo, 
                dto.Descricao, 
                dto.DataInicio, 
                dto.DataFim, 
                dto.Local, 
                dto.MesReferencia, 
                dto.LimiteEntregaMaterial, 
                dto.Status, 
                dto.Observacoes, 
                dto.IdCidade, 
                dto.IdResponsavel
            );

            // 3. Adicionar Relacionamentos N:N (Clientes e Fornecedores)
            if (dto.IdsClientes != null)
            {
                foreach (var idCliente in dto.IdsClientes)
                {
                    evento.AdicionarCliente(new EventoCliente(evento.IdEvento, idCliente));
                }
            }

            if (dto.IdsFornecedores != null)
            {
                foreach (var idFornecedor in dto.IdsFornecedores)
                {
                    evento.AdicionarFornecedor(new EventoFornecedor(evento.IdEvento, idFornecedor));
                }
            }

            // 4. Salvar
            await _eventoRepository.AdicionarAsync(evento);

            _logger.LogInformation("Evento {Codigo} criado com sucesso pelo usuário {UserId}", dto.CodigoEvento, dto.IdResponsavel);
            return ResponseResult.Sucesso("Evento criado com sucesso!", new { Id = evento.IdEvento });
        }
        catch (DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar evento {Codigo}", dto.CodigoEvento);
            return ResponseResult.Erro($"Erro ao criar evento: {ex.Message}");
        }
    }

    public async Task<EventoDto?> ObterPorIdAsync(int idEvento)
    {
        var evento = await _eventoRepository.ObterPorIdCompletoAsync(idEvento);
        if (evento == null) return null;

        return MapearParaDTO(evento);
    }

    public async Task<IEnumerable<EventoDto>> ObterTodosAsync()
    {
        var eventos = await _eventoRepository.ObterTodosAsync();
        return eventos.Select(MapearParaDTO);
    }

    public async Task<ResponseResult> AtualizarAsync(int idEvento, AtualizarEventoDTO dto)
    {
        try
        {
            var evento = await _eventoRepository.ObterPorIdCompletoAsync(idEvento);
            if (evento == null)
                return ResponseResult.Erro("Evento não encontrado.");

            // Atualizar propriedades básicas
            evento.Atualizar(
                evento.CodigoEvento, // Mantém o código original
                dto.Titulo, 
                dto.Descricao, 
                dto.DataInicio, 
                dto.DataFim, 
                dto.Local, 
                dto.MesReferencia, 
                dto.LimiteEntregaMaterial, 
                dto.Status, 
                dto.Observacoes, 
                dto.IdCidade, 
                evento.IdResponsavel // Mantém o responsável original (ou ajuste se necessário)
            );

            // Sincronizar Clientes (Remove os antigos e adiciona os novos)
            // 
            // ou apenas limpar a lista interna se ela for acessível. 
            if (dto.IdsClientes != null)
            {
                
             
                var novosClientes = dto.IdsClientes.Select(id => new EventoCliente(evento.IdEvento, id)).ToList();
                
                // Se o Domain tiver um método para substituir, use-o. 
                // Ex: evento.DefinirClientes(novosClientes);
            }

            if (dto.IdsFornecedores != null)
            {
                var novosFornecedores = dto.IdsFornecedores.Select(id => new EventoFornecedor(evento.IdEvento, id)).ToList();
                // evento.DefinirFornecedores(novosFornecedores);
            }

            await _eventoRepository.AtualizarAsync(evento);
            return ResponseResult.Sucesso("Evento atualizado com sucesso!");
        }
        catch (DomainException ex)
        {
            return ResponseResult.Erro(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar evento {Id}", idEvento);
            return ResponseResult.Erro($"Erro ao atualizar evento: {ex.Message}");
        }
    }

    public async Task<ResponseResult> ExcluirAsync(int idEvento)
    {
        try
        {
            var evento = await _eventoRepository.ObterPorIdAsync(idEvento);
            if (evento == null)
                return ResponseResult.Erro("Evento não encontrado.");

            await _eventoRepository.RemoverAsync(idEvento);
            return ResponseResult.Sucesso("Evento excluído com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir evento {Id}", idEvento);
            return ResponseResult.Erro($"Erro ao excluir evento: {ex.Message}");
        }
    }

    #region Métodos Auxiliares de Mapeamento

    private EventoDto MapearParaDTO(Evento evento)
    {
        return new EventoDto
        {
            IdEvento = evento.IdEvento,
            CodigoEvento = evento.CodigoEvento,
            Titulo = evento.Titulo,
            Descricao = evento.Descricao,
            DataInicio = evento.DataInicio,
            DataFim = evento.DataFim,
            Local = evento.Local,
            MesReferencia = evento.MesReferencia,
            LimiteEntregaMaterial = evento.LimiteEntregaMaterial,
            Status = evento.Status,
            Observacoes = evento.Observacoes,
            
            // Dados relacionados "achatados"
            NomeCidade = evento.Cidade?.NomeCidade,
            NomeResponsavel = evento.Responsavel?.Nome,

            // Listas de relacionamentos
            Clientes = evento.Clientes?.Select(c => new ItemResumoDto
            { 
                Id = c.IdCliente, 
                Nome = c.Cliente?.Nome ?? "Desconhecido" 
            }).ToList() ?? new List<ItemResumoDto>(),

            Fornecedores = evento.Fornecedores?.Select(f => new ItemResumoDto
            { 
                Id = f.IdFornecedor, 
                Nome = f.Fornecedor?.Nome ?? "Desconhecido" 
            }).ToList() ?? new List<ItemResumoDto>()
        };
    }

    #endregion
}