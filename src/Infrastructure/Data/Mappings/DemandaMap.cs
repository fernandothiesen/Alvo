using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class DemandaConfiguration : IEntityTypeConfiguration<Demanda>
{
    public void Configure(EntityTypeBuilder<Demanda> builder)
    {
        builder.HasKey(d => d.IdDemanda);

        builder.Property(d => d.Titulo).HasMaxLength(200).IsRequired();
        builder.Property(d => d.Prioridade).HasMaxLength(50);
        builder.Property(d => d.Status).HasMaxLength(50);

        builder.HasOne(d => d.Evento)
            .WithMany(e => e.Demandas)
            .HasForeignKey(d => d.IdEvento)
            .IsRequired();

        builder.Metadata.FindNavigation(nameof(Demanda.Clientes))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation(nameof(Demanda.Fornecedores))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation(nameof(Demanda.Financeiros))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}