using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;


public class EstadoMap : IEntityTypeConfiguration<Estado>
{
    public void Configure(EntityTypeBuilder<Estado> builder)
    {   
        builder.ToTable("estado");

        builder.HasKey(e => e.IdEstado);

        builder.Property(e => e.IdEstado).HasColumnName("id_estado").UseIdentityColumn();
        builder.Property(e => e.NomeEstado).HasColumnName("nome").HasMaxLength(100).IsRequired();
        builder.Property(e => e.SiglaEstado).HasColumnName("sigla_estado").IsRequired();

        builder.HasOne(e => e.Pais)
            .WithMany()
            .HasForeignKey(e => e.IdPais)
            .IsRequired();
    }
}