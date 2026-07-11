using System.Data.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Data.Mappings;


public class UsuarioRoleMap : IEntityTypeConfiguration<UsuarioRole>
{
    public void Configure(EntityTypeBuilder<UsuarioRole> builder)
    {
        builder.ToTable("usuario_role");

        builder.HasKey(ur => new {ur.IdUsuario, ur.IdRole});

        builder.Property(ur => ur.IdUsuario).HasColumnName("id_usuario");
        builder.Property(ur => ur.IdRole).HasColumnName("id_role");

        builder.HasOne(ur => ur.Usuario).WithMany(u => u.Roles).HasForeignKey(ur => ur.IdUsuario);
        builder.HasOne(ur => ur.Role).WithMany().HasForeignKey(ur => ur.IdRole);
    }

}