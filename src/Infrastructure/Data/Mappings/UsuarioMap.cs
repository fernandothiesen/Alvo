using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace Infrastructure.Data;

public class UsuarioMap : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuario");
        builder.HasKey(u => u.IdUsuario);

        builder.Property(u => u.IdUsuario).HasColumnName("id_usuario");
        builder.Property(u => u.Nome).HasColumnName("nome").IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).HasColumnName("email").IsRequired().HasMaxLength(150);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.SenhaHash).HasColumnName("senha_hash").IsRequired();
        builder.Property(u => u.Ativo).HasColumnName("ativo").HasDefaultValue(true);
        builder.Property(u => u.DataCriacao).HasColumnName("data_criacao").HasDefaultValueSql("NOW()");
        builder.Property(u => u.UltimoLogin).HasColumnName("ultimo_login");

        //Relacionamento 

        builder.HasMany(u => u.Roles).WithOne(ur => ur.Usuario).HasForeignKey(ur => ur.IdUsuario);
        builder.HasMany(u => u.Permissoes).WithOne(up => up.Usuario).HasForeignKey(up => up.IdUsuario);
    }




}