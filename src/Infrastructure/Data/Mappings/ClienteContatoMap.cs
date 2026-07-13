using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class ClienteContatoMap : IEntityTypeConfiguration<ClienteContato>
{
    public void Configure(EntityTypeBuilder<ClienteContato> builder)
    {
        builder.ToTable("cliente_contato");
        builder.HasKey(cc => new { cc.IdCliente, cc.IdContato });

        builder.Property(cc => cc.IdCliente).HasColumnName("id_cliente");
        builder.Property(cc => cc.IdContato).HasColumnName("id_contato");

        builder.HasOne(cc => cc.Cliente).WithMany(c => c.Contatos).HasForeignKey(cc => cc.IdCliente);
        builder.HasOne(cc => cc.Contato).WithMany().HasForeignKey(cc => cc.IdContato);
    }
}