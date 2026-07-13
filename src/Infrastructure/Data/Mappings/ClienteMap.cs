using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.HasKey(c => c.IdCliente);
        builder.Property(c => c.Nome).HasMaxLength(200).IsRequired();

        builder.HasOne(c => c.Cidade)
            .WithMany()
            .HasForeignKey(c => c.IdCidade)
            .IsRequired(false);
    }
}