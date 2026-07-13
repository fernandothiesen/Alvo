using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class ClienteDocumentoMap : IEntityTypeConfiguration<ClienteDocumento>
{
    public void Configure(EntityTypeBuilder<ClienteDocumento> builder)
    {
        builder.ToTable("cliente_documento");
        builder.HasKey(cd => new { cd.IdCliente, cd.IdDocumento });

        builder.Property(cd => cd.IdCliente).HasColumnName("id_cliente");
        builder.Property(cd => cd.IdDocumento).HasColumnName("id_documento");

        builder.HasOne(cd => cd.Cliente).WithMany(c => c.Documentos).HasForeignKey(cd => cd.IdCliente);
        builder.HasOne(cd => cd.Documento).WithMany().HasForeignKey(cd => cd.IdDocumento);
    }
}