using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class DemandaClienteMap : IEntityTypeConfiguration<DemandaCliente>
{
    public void Configure(EntityTypeBuilder<DemandaCliente> builder)
    {
        builder.ToTable("demanda_cliente");
        builder.HasKey(dc => new { dc.IdDemanda, dc.IdCliente });

        builder.Property(dc => dc.IdDemanda).HasColumnName("id_demanda");
        builder.Property(dc => dc.IdCliente).HasColumnName("id_cliente");

        builder.HasOne(dc => dc.Demanda).WithMany(d => d.Clientes).HasForeignKey(dc => dc.IdDemanda);
        builder.HasOne(dc => dc.Cliente).WithMany(c => c.Demandas).HasForeignKey(dc => dc.IdCliente);
    }
}