using System.Security.Cryptography.Xml;
using System.Text;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder();

//DI Services

// Infrastructure (DB, Repositories, Auth services)
builder.Services.AddInfrastructure(builder.Configuration);

// Application services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IPaisService, PaisService>();
builder.Services.AddScoped<IEstadoService, EstadoService>();
builder.Services.AddScoped<ICidadeService, CidadeService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
// JWT && Auth configurations
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)),


        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.Name
    };

    // Recebe token por string query (útil para Swagger/WebSockets)
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// Authorization config
builder.Services.AddAuthorization(options =>
{
    // ========================================
    // POLÍTICAS BASEADAS EM ROLES (Simples)
    // ========================================
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("AdminOuGerente", policy => policy.RequireRole("Admin", "Gerente"));

    // ========================================
    // POLÍTICAS BASEADAS EM PERMISSÕES (Granular)
    // ========================================
    
    // USUÁRIOS
    options.AddPolicy("GerenciarUsuarios", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") ||
            context.User.HasClaim(c => c.Type == "permissao" && c.Value == "GerenciarUsuarios")));

    // ROLES
    options.AddPolicy("GerenciarRoles", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") ||
            context.User.HasClaim(c => c.Type == "permissao" && c.Value == "GerenciarRoles")));

    // EVENTOS
    options.AddPolicy("GerenciarEventos", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") ||
            context.User.HasClaim(c => c.Type == "permissao" && c.Value == "GerenciarEventos")));

    options.AddPolicy("VisualizarEventos", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") ||
            context.User.HasClaim(c => c.Type == "permissao" && c.Value == "VisualizarEventos")));

    // DEMANDAS
    options.AddPolicy("GerenciarDemandas", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") ||
            context.User.HasClaim(c => c.Type == "permissao" && c.Value == "GerenciarDemandas")));

    // CLIENTES
    options.AddPolicy("GerenciarClientes", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") ||
            context.User.HasClaim(c => c.Type == "permissao" && c.Value == "GerenciarClientes")));

    // FORNECEDORES
    options.AddPolicy("GerenciarFornecedores", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") ||
            context.User.HasClaim(c => c.Type == "permissao" && c.Value == "GerenciarFornecedores")));

    // FINANCEIRO
    options.AddPolicy("AcessarFinanceiro", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") ||
            context.User.HasClaim(c => c.Type == "permissao" && c.Value == "AcessarFinanceiro")));

    options.AddPolicy("GerenciarFinanceiro", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") ||
            context.User.HasClaim(c => c.Type == "permissao" && c.Value == "GerenciarFinanceiro")));

    // CONTAS BANCÁRIAS
    options.AddPolicy("GerenciarContasBancarias", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") ||
            context.User.HasClaim(c => c.Type == "permissao" && c.Value == "GerenciarContasBancarias")));


});


// Swagger config
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new Microsoft.OpenApi.Models.OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, Microsoft.OpenApi.Models.OpenApiSecurityScheme>();

        var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header usando o esquema Bearer. Exemplo: 'Bearer {token}'"
        };

        document.Components.SecuritySchemes.Add("Bearer", securityScheme);

        var securityRequirement = new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        };

        if (document.Paths != null)
        {
            foreach (var apiDescription in context.DescriptionGroups.SelectMany(g => g.Items))
            {
                var hasAllowAnonymous = apiDescription.ActionDescriptor.EndpointMetadata
                    .OfType<AllowAnonymousAttribute>().Any();

                if (hasAllowAnonymous)
                    continue;

                var path = document.Paths[apiDescription.RelativePath!.Insert(0, "/")];
                var httpMethod = Enum.Parse<Microsoft.OpenApi.Models.OperationType>(apiDescription.HttpMethod!, ignoreCase: true);

                if (path.Operations.TryGetValue(httpMethod, out var operation))
                {
                    operation.Security ??= new List<Microsoft.OpenApi.Models.OpenApiSecurityRequirement>();
                    operation.Security.Add(securityRequirement);
                }
            }
        }

        return Task.CompletedTask;
    });
});

// Controllers config
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Mantém PascalCase do C# se desejado
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Cors config
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://26.159.126.189:3000")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});



var app = builder.Build();

//pipeline http request


app.UseHttpsRedirection(); 

// Enable swagger development only
if (app.Environment.IsDevelopment())
{

    app.MapOpenApi();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "Sistema de Eventos API v1"); 
        c.RoutePrefix = "swagger";
    });
}


app.UseCors("AllowAll"); // Altere para "AllowFrontend" em produção

// Authentication && Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();