using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class EventoClienteMap : IEntityTypeConfiguration<EventoCliente>
{
    public void Configure(EntityTypeBuilder<EventoCliente> builder)
    {
        builder.ToTable("evento_cliente");
        builder.HasKey(ec => new { ec.IdEvento, ec.IdCliente });

        builder.Property(ec => ec.IdEvento).HasColumnName("id_evento");
        builder.Property(ec => ec.IdCliente).HasColumnName("id_cliente");

        builder.HasOne(ec => ec.Evento).WithMany(e => e.Clientes).HasForeignKey(ec => ec.IdEvento);
        builder.HasOne(ec => ec.Cliente).WithMany(c => c.Eventos).HasForeignKey(ec => ec.IdCliente);
    }
}