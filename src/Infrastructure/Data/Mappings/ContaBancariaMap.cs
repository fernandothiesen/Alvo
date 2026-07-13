using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class ContaBancariaMap : IEntityTypeConfiguration<ContaBancaria>
{
    public void Configure(EntityTypeBuilder<ContaBancaria> builder)
    {
        builder.ToTable("conta_bancaria");
        builder.HasKey(c => c.IdContaBancaria);

        builder.Property(c => c.IdContaBancaria).HasColumnName("id_conta").UseIdentityColumn();
        builder.Property(c => c.Banco).HasColumnName("banco").IsRequired().HasMaxLength(100);
        builder.Property(c => c.Agencia).HasColumnName("agencia").IsRequired().HasMaxLength(20);
        builder.Property(c => c.NumeroConta).HasColumnName("numero_conta").IsRequired().HasMaxLength(20);
        builder.Property(c => c.TipoConta).HasColumnName("tipo_conta").IsRequired().HasMaxLength(50);
        builder.Property(c => c.Titular).HasColumnName("titular").IsRequired().HasMaxLength(150);
        builder.Property(c => c.Ativo).HasColumnName("ativo").HasDefaultValue(true);
    }
}