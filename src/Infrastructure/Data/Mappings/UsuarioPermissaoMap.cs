using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class UsuarioPermissaoMap : IEntityTypeConfiguration<UsuarioPermissao>
{
    public void Configure(EntityTypeBuilder<UsuarioPermissao> builder)
    {
        builder.ToTable("usuario_permissao");
        builder.HasKey(up => new { up.IdUsuario, up.IdPermissao });

        builder.Property(up => up.IdUsuario).HasColumnName("id_usuario");
        builder.Property(up => up.IdPermissao).HasColumnName("id_permissao");

        builder.HasOne(up => up.Usuario).WithMany(u => u.Permissoes).HasForeignKey(up => up.IdUsuario);
        builder.HasOne(up => up.Permissao).WithMany().HasForeignKey(up => up.IdPermissao);
    }
}