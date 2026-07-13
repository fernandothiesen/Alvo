using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class FinanceiroMap : IEntityTypeConfiguration<Financeiro>
{
    public void Configure(EntityTypeBuilder<Financeiro> builder)
    {
        builder.ToTable("financeiro");
        builder.HasKey(f => f.IdFinanceiro);

        builder.Property(f => f.IdFinanceiro).HasColumnName("id_financeiro").UseIdentityColumn();
        builder.Property(f => f.Tipo).HasColumnName("tipo").IsRequired().HasMaxLength(50);
        builder.Property(f => f.Valor).HasColumnName("valor").HasColumnType("numeric(12,2)").IsRequired();
        builder.Property(f => f.DataMovimentacao).HasColumnName("data_movimentacao").IsRequired();
        builder.Property(f => f.Descricao).HasColumnName("descricao").HasMaxLength(500);
        
        builder.Property(f => f.IdEvento).HasColumnName("id_evento");
        builder.Property(f => f.IdDemanda).HasColumnName("id_demanda");
        builder.Property(f => f.IdFornecedor).HasColumnName("id_fornecedor");
        builder.Property(f => f.IdCliente).HasColumnName("id_cliente");
        builder.Property(f => f.IdFormaPagamento).HasColumnName("id_forma_pagamento").IsRequired();
        builder.Property(f => f.IdConta).HasColumnName("id_conta");

        // Relacionamentos
        builder.HasOne(f => f.Evento).WithMany(e => e.Financeiros).HasForeignKey(f => f.IdEvento);
        builder.HasOne(f => f.Demanda).WithMany(d => d.Financeiros).HasForeignKey(f => f.IdDemanda);
        builder.HasOne(f => f.Fornecedor).WithMany().HasForeignKey(f => f.IdFornecedor);
        builder.HasOne(f => f.Cliente).WithMany().HasForeignKey(f => f.IdCliente);
        builder.HasOne(f => f.FormaPagamento).WithMany().HasForeignKey(f => f.IdFormaPagamento);
        builder.HasOne(f => f.ContaBancaria).WithMany().HasForeignKey(f => f.IdConta);
    }
}