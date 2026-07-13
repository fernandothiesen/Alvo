using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;
public class PaisMap : IEntityTypeConfiguration<Pais>
{
    public void Configure(EntityTypeBuilder<Pais> builder)
    {

        builder.ToTable("pais");

        builder.HasKey(p => p.IdPais);


        builder.Property(p => p.IdPais).HasColumnName("id_pais").UseIdentityColumn();
        builder.Property(p => p.NomePais).HasColumnName("nome_pais").HasMaxLength(100).IsRequired();
        builder.Property(p => p.CodigoIso).HasColumnName("codigo_iso").IsRequired();
    }

}