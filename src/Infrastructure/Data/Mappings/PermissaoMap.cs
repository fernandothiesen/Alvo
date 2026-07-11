using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Data.Mappings;


public class PermissaoMap : IEntityTypeConfiguration<Permissao>
{
    public void Configure(EntityTypeBuilder<Permissao> builder)
    {
        builder.ToTable("permissao");
        builder.HasKey(p => p.IdPermissao);


        builder.Property(p => p.IdPermissao).HasColumnName("id_permissao").UseIdentityColumn();
        builder.Property(p => p.NomePermissao).HasColumnName("nome_permissao").IsRequired().HasMaxLength(100);
        builder.Property(p => p.Descricao).HasColumnName("descricao").HasMaxLength(200);
    }
}