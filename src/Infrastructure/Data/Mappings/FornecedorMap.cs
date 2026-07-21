using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class FornecedorMap : IEntityTypeConfiguration<Fornecedor>
{
    public void Configure(EntityTypeBuilder<Fornecedor> builder)
    {
        builder.ToTable("fornecedor");
        builder.HasKey(f => f.IdFornecedor);

        builder.Property(f => f.IdFornecedor)
            .HasColumnName("id_fornecedor")
            .UseIdentityColumn();

        builder.Property(f => f.Nome)
            .HasColumnName("nome")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(f => f.Ativo)
            .HasColumnName("ativo")
            .HasDefaultValue(true);

        builder.Property(f => f.DataCadastro)
            .HasColumnName("data_cadastro")
            .HasDefaultValueSql("NOW()");

        builder.Property(f => f.IdCidade)
            .HasColumnName("id_cidade");

        // Relacionamento com Cidade
        builder.HasOne(f => f.Cidade)
            .WithMany()
            .HasForeignKey(f => f.IdCidade);

        // Relacionamentos N:N (apenas para navegação)
        builder.HasMany(f => f.Contatos)
            .WithOne(fc => fc.Fornecedor)
            .HasForeignKey(fc => fc.IdFornecedor);

        builder.HasMany(f => f.Documentos)
            .WithOne(fd => fd.Fornecedor)
            .HasForeignKey(fd => fd.IdFornecedor);

        builder.HasMany(f => f.ContasBancarias)
            .WithOne(fcb => fcb.Fornecedor)
            .HasForeignKey(fcb => fcb.IdFornecedor);

        builder.HasMany(f => f.Servicos)
            .WithOne(fs => fs.Fornecedor)
            .HasForeignKey(fs => fs.IdFornecedor);

        builder.HasMany(f => f.Eventos)
            .WithOne(fe => fe.Fornecedor)
            .HasForeignKey(fe => fe.IdFornecedor);

        builder.HasMany(f => f.Demandas)
            .WithOne(fd => fd.Fornecedor)
            .HasForeignKey(fd => fd.IdFornecedor);
    }
}