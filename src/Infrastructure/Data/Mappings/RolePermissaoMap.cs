using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace Infrastructure.Data.Mappings;


public class RolePermissaoMap : IEntityTypeConfiguration<RolePermissao>
{
    
    public void Configure(EntityTypeBuilder<RolePermissao> builder)
    {
        builder.ToTable("role_permissao");

        builder.HasKey(rp => new { rp.IdRole, rp.IdPermissao});

        builder.HasOne(rp => rp.Role)
            .WithMany(r => r.Permissoes)
            .HasForeignKey(rp => rp.IdRole);

        builder.HasOne(rp => rp.Permissao)
            .WithMany()
            .HasForeignKey(rp => rp.IdPermissao);
    }
}