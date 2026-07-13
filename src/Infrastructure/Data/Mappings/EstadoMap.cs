using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;


public class EstadoMap : IEntityTypeConfiguration<Estado>
{
    public void Configure(EntityTypeBuilder<Estado> builder)
    {
        builder.HasKey(e => e.IdEstado);
        builder.Property(e => e.NomeEstado).HasMaxLength(100).IsRequired();
        builder.Property(e => e.SiglaEstado).IsRequired();

        builder.HasOne(e => e.Pais)
            .WithMany()
            .HasForeignKey(e => e.IdPais)
            .IsRequired();
    }
}