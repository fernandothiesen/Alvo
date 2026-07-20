using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class EventoMap : IEntityTypeConfiguration<Evento>
{
    public void Configure(EntityTypeBuilder<Evento> builder)
    {
        builder.ToTable("evento");
        builder.HasKey(e => e.IdEvento);

        // Propriedades simples
        builder.Property(e => e.IdEvento)
            .HasColumnName("id_evento")
            .UseIdentityColumn();

        builder.Property(e => e.CodigoEvento)
            .HasColumnName("codigo_evento")
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(e => e.CodigoEvento).IsUnique();

        builder.Property(e => e.Titulo)
            .HasColumnName("titulo")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(1000);

        builder.Property(e => e.DataInicio)
            .HasColumnName("data_inicio");

        builder.Property(e => e.DataFim)
            .HasColumnName("data_fim");

        builder.Property(e => e.Local)
            .HasColumnName("local")
            .HasMaxLength(200);

        builder.Property(e => e.MesReferencia)
            .HasColumnName("mes_referencia")
            .HasMaxLength(50);

        builder.Property(e => e.LimiteEntregaMaterial)
            .HasColumnName("limite_entrega_material");

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasMaxLength(50);

        builder.Property(e => e.Observacoes)
            .HasColumnName("observacoes")
            .HasMaxLength(1000);

        builder.Property(e => e.IdCidade)
            .HasColumnName("id_cidade");

        builder.Property(e => e.IdResponsavel)
            .HasColumnName("id_usuario_responsavel"); 

        // Relacionamentos
        builder.HasOne(e => e.Cidade)
            .WithMany()
            .HasForeignKey(e => e.IdCidade);

        builder.HasOne(e => e.Responsavel)
            .WithMany()
            .HasForeignKey(e => e.IdResponsavel);

        builder.HasMany(e => e.Clientes)
            .WithOne(ec => ec.Evento)
            .HasForeignKey(ec => ec.IdEvento);

        builder.HasMany(e => e.Fornecedores)
            .WithOne(ef => ef.Evento)
            .HasForeignKey(ef => ef.IdEvento);

        builder.HasMany(e => e.Demandas)
            .WithOne(d => d.Evento)
            .HasForeignKey(d => d.IdEvento);

        builder.HasMany(e => e.Financeiros)
            .WithOne(f => f.Evento)
            .HasForeignKey(f => f.IdEvento);
    }
}