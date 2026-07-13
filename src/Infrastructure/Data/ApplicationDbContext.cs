using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // ==========================================
    // 1. DbSet das Entidades Principais (Core)
    // ==========================================
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permissao> Permissoes { get; set; }

    // ==========================================
    // 2. DbSet das Entidades de Negócio
    // ==========================================
    public DbSet<Pais> Paises { get; set; }
    public DbSet<Estado> Estados { get; set; }
    public DbSet<Cidade> Cidades { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<Demanda> Demandas { get; set; }

    // ==========================================
    // 3. DbSet das Entidades Auxiliares / Cadastros
    // ==========================================
    public DbSet<Servico> Servicos { get; set; }
    public DbSet<Documento> Documentos { get; set; }
    public DbSet<Contato> Contatos { get; set; }
    public DbSet<FormaPagamento> FormasPagamento { get; set; }
    public DbSet<ContaBancaria> ContasBancarias { get; set; }
    public DbSet<Financeiro> Financeiros { get; set; }
    public DbSet<AtividadeSistema> AtividadesSistema { get; set; }

    // ==========================================
    // 4. DbSet das Tabelas de Relacionamento (N:N)
    // ==========================================
    
    // Autenticação e Permissões
    public DbSet<UsuarioRole> UsuarioRoles { get; set; }
    public DbSet<RolePermissao> RolePermissoes { get; set; }
    public DbSet<UsuarioPermissao> UsuarioPermissoes { get; set; }

    // Eventos e Demandas
    public DbSet<EventoCliente> EventoClientes { get; set; }
    public DbSet<EventoFornecedor> EventoFornecedores { get; set; }
    public DbSet<DemandaCliente> DemandaClientes { get; set; }
    public DbSet<DemandaFornecedor> DemandaFornecedores { get; set; }

    // Contatos e Documentos
    public DbSet<ClienteContato> ClienteContatos { get; set; }
    public DbSet<FornecedorContato> FornecedorContatos { get; set; }
    public DbSet<UsuarioContato> UsuarioContatos { get; set; }
    public DbSet<ClienteDocumento> ClienteDocumentos { get; set; }
    public DbSet<FornecedorDocumento> FornecedorDocumentos { get; set; }

    // Específicos de Fornecedor
    public DbSet<FornecedorContaBancaria> FornecedorContasBancarias { get; set; }
    public DbSet<FornecedorServico> FornecedorServicos { get; set; }

    // ==========================================
    // 5. Configuração do Modelo (Mappings)
    // ==========================================
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica automaticamente todos os arquivos de mapping (IEntityTypeConfiguration) desta assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}