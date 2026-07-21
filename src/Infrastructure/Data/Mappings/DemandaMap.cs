using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class DemandaMap : IEntityTypeConfiguration<Demanda>
{
    public void Configure(EntityTypeBuilder<Demanda> builder)
    {
        builder.ToTable("demanda");
        builder.HasKey(d => d.IdDemanda);

        builder.Property(d => d.IdDemanda).HasColumnName("id_demanda").UseIdentityColumn();
        builder.Property(d => d.IdEvento).HasColumnName("id_evento");
        builder.Property(d => d.Titulo).HasColumnName("titulo").IsRequired().HasMaxLength(200);
        builder.Property(d => d.Descricao).HasColumnName("descricao").HasMaxLength(1000);
        builder.Property(d => d.Prioridade).HasColumnName("prioridade").HasMaxLength(50);
        builder.Property(d => d.Status).HasColumnName("status").HasMaxLength(50);
        builder.Property(d => d.DataCriacao).HasColumnName("data_criacao").HasDefaultValueSql("NOW()");
        builder.Property(d => d.DataConclusao).HasColumnName("data_conclusao");

        // Relacionamentos
        builder.HasOne(d => d.Evento).WithMany(e => e.Demandas).HasForeignKey(d => d.IdEvento);
        
        builder.HasMany(d => d.Clientes).WithOne(dc => dc.Demanda).HasForeignKey(dc => dc.IdDemanda);
        builder.HasMany(d => d.Fornecedores).WithOne(df => df.Demanda).HasForeignKey(df => df.IdDemanda);
        builder.HasMany(d => d.Financeiros).WithOne(f => f.Demanda).HasForeignKey(f => f.IdDemanda);
    }
}