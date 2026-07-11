using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options){}


    //DbSet das entidades principais

    public DbSet<Usuario> Usuarios {get; set;}
    public DbSet<Role> Roles {get; set;}
    public DbSet<Permissao> Permissoes {get; set;}


    // DbSet entidades de negocio 

    public DbSet<Pais> Paises {get; set;}
    public DbSet<Estado> Estados {get; set;}
    public DbSet<Cidade> Cidades {get; set;}
    public DbSet<Cliente> Clientes {get; set;}
    public DbSet<Fornecedor> Fornecedores {get; set;}
    public DbSet<Evento> Eventos {get; set;}
    public DbSet<Demanda> Demandas {get; set;}


    //DbSet tabelas relacionamento 

    public DbSet<UsuarioRole> UsuarioRoles {get; set;}
    public DbSet<UsuarioPermissao> UsuarioPermissoes {get; set;}
    public DbSet<RolePermissao> RolePermissoes {get; set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Aplica automaticamente todos os arquivos de mapping 

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}