using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;


public class FormaPagamentoMap : IEntityTypeConfiguration<FormaPagamento>
{
    public void Configure(EntityTypeBuilder<FormaPagamento> builder)
    {
        builder.ToTable("forma_pagamento");
        builder.HasKey(f => f.IdPagamento);


        builder.Property(f => f.IdPagamento).HasColumnName("id_forma_pagamento").UseIdentityColumn();
        builder.Property(f => f.TipoFormaPagamento).HasColumnName("tipo_forma_pagamento").IsRequired().HasMaxLength(50);
        builder.Property(f => f.Descricao).HasColumnName("descricao").HasMaxLength(200);        
    }

}