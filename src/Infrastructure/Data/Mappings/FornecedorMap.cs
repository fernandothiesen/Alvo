using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor>
{
    public void Configure(EntityTypeBuilder<Fornecedor> builder)
    {
        builder.HasKey(f => f.IdFornecedor);
        builder.Property(f => f.Nome).HasMaxLength(200).IsRequired();

        builder.HasOne(f => f.Cidade)
            .WithMany()
            .HasForeignKey(f => f.IdCidade)
            .IsRequired(false);
    }
}