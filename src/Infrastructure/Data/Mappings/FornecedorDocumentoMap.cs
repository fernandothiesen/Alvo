using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class FornecedorDocumentoMap : IEntityTypeConfiguration<FornecedorDocumento>
{
    public void Configure(EntityTypeBuilder<FornecedorDocumento> builder)
    {
        builder.ToTable("fornecedor_documento");
        builder.HasKey(fd => new { fd.IdFornecedor, fd.IdDocumento });

        builder.Property(fd => fd.IdFornecedor).HasColumnName("id_fornecedor");
        builder.Property(fd => fd.IdDocumento).HasColumnName("id_documento");

        builder.HasOne(fd => fd.Fornecedor).WithMany(f => f.Documentos).HasForeignKey(fd => fd.IdFornecedor);
        builder.HasOne(fd => fd.Documento).WithMany().HasForeignKey(fd => fd.IdDocumento);
    }
}