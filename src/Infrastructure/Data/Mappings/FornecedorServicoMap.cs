using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class FornecedorServicoMap : IEntityTypeConfiguration<FornecedorServico>
{
    public void Configure(EntityTypeBuilder<FornecedorServico> builder)
    {
        builder.ToTable("fornecedor_servico");
        builder.HasKey(fs => new { fs.IdFornecedor, fs.IdServico });

        builder.Property(fs => fs.IdFornecedor).HasColumnName("id_fornecedor");
        builder.Property(fs => fs.IdServico).HasColumnName("id_servico");

        builder.HasOne(fs => fs.Fornecedor).WithMany(f => f.Servicos).HasForeignKey(fs => fs.IdFornecedor);
        builder.HasOne(fs => fs.Servico).WithMany().HasForeignKey(fs => fs.IdServico);
    }
}