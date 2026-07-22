using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class DemandaFornecedorMap : IEntityTypeConfiguration<DemandaFornecedor>
{
    public void Configure(EntityTypeBuilder<DemandaFornecedor> builder)
    {
        builder.ToTable("demanda_fornecedor");
        builder.HasKey(df => new { df.IdDemanda, df.IdFornecedor });

        builder.Property(df => df.IdDemanda).HasColumnName("id_demanda");
        builder.Property(df => df.IdFornecedor).HasColumnName("id_fornecedor");

        builder.HasOne(df => df.Demanda).WithMany(d => d.Fornecedores).HasForeignKey(df => df.IdDemanda);
        builder.HasOne(df => df.Fornecedor).WithMany(f => f.Demandas).HasForeignKey(df => df.IdFornecedor);
    }
}