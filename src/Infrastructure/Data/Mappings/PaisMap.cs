using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;
public class PaisMap : IEntityTypeConfiguration<Pais>
{
    public void Configure(EntityTypeBuilder<Pais> builder)
    {
        builder.HasKey(p => p.IdPais);
        builder.Property(p => p.NomePais).HasMaxLength(100).IsRequired();
        builder.Property(p => p.CodigoIso).IsRequired();
    }

}