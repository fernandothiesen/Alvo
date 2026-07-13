using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class UsuarioContatoMap : IEntityTypeConfiguration<UsuarioContato>
{
    public void Configure(EntityTypeBuilder<UsuarioContato> builder)
    {
        builder.ToTable("usuario_contato");
        builder.HasKey(uc => new { uc.IdUsuario, uc.IdContato });

        builder.Property(uc => uc.IdUsuario).HasColumnName("id_usuario");
        builder.Property(uc => uc.IdContato).HasColumnName("id_contato");

        builder.HasOne(uc => uc.Usuario).WithMany(u => u.Contatos).HasForeignKey(uc => uc.IdUsuario);
        builder.HasOne(uc => uc.Contato).WithMany().HasForeignKey(uc => uc.IdContato);
    }
}