using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CidadeConfiguration : IEntityTypeConfiguration<Cidade>
{
    public void Configure(EntityTypeBuilder<Cidade> builder)
    {
        builder.HasKey(c => c.IdCidade);

        builder.Property(c => c.NomeCidade).HasMaxLength(100).IsRequired();

        builder.HasOne(c => c.Estado)
            .WithMany()
            .HasForeignKey(c => c.IdEstado)
            .IsRequired();
    }
}