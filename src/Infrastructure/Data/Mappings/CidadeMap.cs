using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class CidadeMap : IEntityTypeConfiguration<Cidade>
{
    public void Configure(EntityTypeBuilder<Cidade> builder)
    {
        builder.ToTable("cidade");
        builder.HasKey(c => c.IdCidade);

        builder.Property(c => c.IdCidade)
            .HasColumnName("id_cidade")
            .UseIdentityColumn();

        builder.Property(c => c.IdEstado)
            .HasColumnName("id_estado"); // ← snake_case correto

        builder.Property(c => c.NomeCidade)
            .HasColumnName("nome_cidade") // ← snake_case correto (NÃO "nome")
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(c => c.Estado)
            .WithMany(e => e.Cidades)
            .HasForeignKey(c => c.IdEstado);
    }
}