using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace Infrastructure.Mappings;


public class RolePermissaoMap : IEntityTypeConfiguration<RolePermissao>
{
    
    public void Configure(EntityTypeBuilder<RolePermissao> builder)
    {
        builder.HasKey(rp => new { rp.IdRole, rp.IdPermissao});

        builder.HasOne(rp => rp.Role)
            .WithMany()
            .HasForeignKey(rp => rp.IdRole);

        builder.HasOne(rp => rp.Permissao)
            .WithMany()
            .HasForeignKey(rp => rp.IdPermissao);
    }
}