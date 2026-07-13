using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class EventoFornecedorMap : IEntityTypeConfiguration<EventoFornecedor>
{
    public void Configure(EntityTypeBuilder<EventoFornecedor> builder)
    {
        builder.ToTable("evento_fornecedor");
        builder.HasKey(ef => new { ef.IdEvento, ef.IdFornecedor });

        builder.Property(ef => ef.IdEvento).HasColumnName("id_evento");
        builder.Property(ef => ef.IdFornecedor).HasColumnName("id_fornecedor");

        builder.HasOne(ef => ef.Evento).WithMany(e => e.Fornecedores).HasForeignKey(ef => ef.IdEvento);
        builder.HasOne(ef => ef.Fornecedor).WithMany(f => f.Eventos).HasForeignKey(ef => ef.IdFornecedor);
    }
}