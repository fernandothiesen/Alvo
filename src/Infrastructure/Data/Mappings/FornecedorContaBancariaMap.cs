using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class FornecedorContaBancariaMap : IEntityTypeConfiguration<FornecedorContaBancaria>
{
    public void Configure(EntityTypeBuilder<FornecedorContaBancaria> builder)
    {
        builder.ToTable("fornecedor_conta_bancaria");
        builder.HasKey(fcb => new { fcb.IdFornecedor, fcb.IdConta });

        builder.Property(fcb => fcb.IdFornecedor).HasColumnName("id_fornecedor");
        builder.Property(fcb => fcb.IdConta).HasColumnName("id_conta");

        builder.HasOne(fcb => fcb.Fornecedor).WithMany(f => f.ContasBancarias).HasForeignKey(fcb => fcb.IdFornecedor);
        builder.HasOne(fcb => fcb.Conta).WithMany().HasForeignKey(fcb => fcb.IdConta);
    }
}