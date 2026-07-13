using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class ServicoMap : IEntityTypeConfiguration<Servico>
{
    public void Configure(EntityTypeBuilder<Servico> builder)
    {
        
        builder.ToTable("servico"); 
        
        builder.HasKey(s => s.IdServico);

        builder.Property(s => s.IdServico)
            .HasColumnName("id_servico")
            .UseIdentityColumn(); 
            
        builder.Property(s => s.NomeServico)
            .HasColumnName("nome_servico")
            .IsRequired(); 
            
        builder.Property(s => s.Descricao)
            .HasColumnName("descricao"); 
    }
}