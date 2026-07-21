using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class EstadoMap : IEntityTypeConfiguration<Estado>
{
    public void Configure(EntityTypeBuilder<Estado> builder)
    {
        builder.ToTable("estado");
        builder.HasKey(e => e.IdEstado);

        builder.Property(e => e.IdEstado)
            .HasColumnName("id_estado")
            .UseIdentityColumn();

        builder.Property(e => e.IdPais)
            .HasColumnName("id_pais");

        builder.Property(e => e.NomeEstado)
            .HasColumnName("nome_estado")  // ← CRUCIAL: deve ser "nome_estado", não "nome"
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.SiglaEstado)
            .HasColumnName("sigla_estado")
            .IsRequired()
            .HasMaxLength(2);

        // Relacionamento com País
        builder.HasOne(e => e.Pais)
            .WithMany(p => p.Estados)
            .HasForeignKey(e => e.IdPais);
    }
}