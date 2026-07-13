using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;


public class ContatoMap : IEntityTypeConfiguration<Contato>
{
    public void Configure(EntityTypeBuilder<Contato> builder)
    {
        builder.ToTable("contato");
        builder.HasKey(c => c.IdContato);

        builder.Property(c => c.IdContato).HasColumnName("id_contato").UseIdentityColumn();
        builder.Property(c => c.TipoContato).HasColumnName("tipo_contato").IsRequired().HasMaxLength(50);
        builder.Property(c => c.ValorContato).HasColumnName("valor_contato").IsRequired().HasMaxLength(100);
        builder.Property(c => c.Observacao).HasColumnName("observacao").HasMaxLength(200);
        builder.Property(c => c.Ativo).HasColumnName("ativo").HasDefaultValue(true);

    }
}