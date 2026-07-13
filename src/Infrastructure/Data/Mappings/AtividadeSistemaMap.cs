using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class AtividadeSistemaMap : IEntityTypeConfiguration<AtividadeSistema>
{
    public void Configure(EntityTypeBuilder<AtividadeSistema> builder)
    {
        builder.ToTable("atividade_sistema");
        builder.HasKey(a => a.IdAtividadeSistema);

        builder.Property(a => a.IdAtividadeSistema).HasColumnName("id_atividade").UseIdentityColumn();
        builder.Property(a => a.IdUsuario).HasColumnName("id_usuario").IsRequired();
        builder.Property(a => a.TipoAtividade).HasColumnName("tipo_atividade").IsRequired().HasMaxLength(100);
        builder.Property(a => a.Descricao).HasColumnName("descricao").HasMaxLength(500);
        builder.Property(a => a.DataHora).HasColumnName("data_hora").HasDefaultValueSql("NOW()");

        builder.HasOne(a => a.Usuario).WithMany().HasForeignKey(a => a.IdUsuario);
    }
}