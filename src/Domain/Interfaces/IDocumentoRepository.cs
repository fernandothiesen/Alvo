using Domain.Entities;

namespace Domain.Interfaces;

public interface IDocumentoRepository : IRepository<Documento>
{
    Task<IEnumerable<Documento>> ObterAtivosAsync();
    Task<IEnumerable<Documento>> ObterPorTipoAsync(string tipoDocumento);
}