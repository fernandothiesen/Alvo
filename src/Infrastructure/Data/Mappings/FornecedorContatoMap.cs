using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class FornecedorContatoMap : IEntityTypeConfiguration<FornecedorContato>
{
    public void Configure(EntityTypeBuilder<FornecedorContato> builder)
    {
        builder.ToTable("fornecedor_contato");
        builder.HasKey(fc => new { fc.IdFornecedor, fc.IdContato });

        builder.Property(fc => fc.IdFornecedor).HasColumnName("id_fornecedor");
        builder.Property(fc => fc.IdContato).HasColumnName("id_contato");

        builder.HasOne(fc => fc.Fornecedor).WithMany(f => f.Contatos).HasForeignKey(fc => fc.IdFornecedor);
        builder.HasOne(fc => fc.Contato).WithMany().HasForeignKey(fc => fc.IdContato);
    }
}