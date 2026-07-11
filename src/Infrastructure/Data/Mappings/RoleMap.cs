using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Data.Mappings;


public class RoleMap : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("role");
        builder.HasKey(r => r.IdRole);

        builder.Property(r => r.IdRole).HasColumnName("id_role").UseIdentityColumn();
        builder.Property(r => r.NomeRole).HasColumnName("nome_role").IsRequired().HasMaxLength(50);
        builder.Property(r => r.Descricao).HasColumnName("descricao").HasMaxLength(100);

        builder.HasMany(r => r.Permissoes).WithOne(rp => rp.Role).HasForeignKey(rp => rp.IdRole);
    }
}