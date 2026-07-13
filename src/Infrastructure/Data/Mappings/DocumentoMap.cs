    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace Infrastructure.Data.Mappings;

public class DocumentoMap : IEntityTypeConfiguration<Documento>
{
    public void Configure(EntityTypeBuilder<Documento> builder)
    {
        builder.ToTable("documento");
        builder.HasKey(d => d.IdDocumento);

        builder.Property(d => d.IdDocumento).HasColumnName("id_documento").UseIdentityColumn();
        builder.Property(d => d.TipoDocumento).HasColumnName("tipo_documento").IsRequired().HasMaxLength(50);
        builder.Property(d => d.ValorDocumento).HasColumnName("valor_documento").IsRequired().HasMaxLength(100);
        builder.Property(d => d.Ativo).HasColumnName("ativo").HasDefaultValue(true);
        builder.Property(d => d.DataValidacao).HasColumnName("data_validacao");
    }
}