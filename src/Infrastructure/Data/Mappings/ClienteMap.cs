using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class ClienteMap : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("cliente");
        builder.HasKey(c => c.IdCliente);

        builder.Property(c => c.IdCliente)
            .HasColumnName("id_cliente")
            .UseIdentityColumn();

        builder.Property(c => c.Nome)
            .HasColumnName("nome")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Ativo)
            .HasColumnName("ativo")
            .HasDefaultValue(true);

        builder.Property(c => c.DataCadastro)
            .HasColumnName("data_cadastro")
            .HasDefaultValueSql("NOW()");

        builder.Property(c => c.IdCidade)
            .HasColumnName("id_cidade");

        // Relacionamento com Cidade
        builder.HasOne(c => c.Cidade)
            .WithMany()
            .HasForeignKey(c => c.IdCidade);

        // Relacionamentos N:N (apenas para navegação)
        builder.HasMany(c => c.Contatos)
            .WithOne(cc => cc.Cliente)
            .HasForeignKey(cc => cc.IdCliente);

        builder.HasMany(c => c.Documentos)
            .WithOne(cd => cd.Cliente)
            .HasForeignKey(cd => cd.IdCliente);

        builder.HasMany(c => c.Eventos)
            .WithOne(ec => ec.Cliente)
            .HasForeignKey(ec => ec.IdCliente);

        builder.HasMany(c => c.Demandas)
            .WithOne(dc => dc.Cliente)
            .HasForeignKey(dc => dc.IdCliente);
    }
}