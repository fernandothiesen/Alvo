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


var builder = WebApplication.CreateBuilder();

//DI Services

// Infrastructure (DB, Repositories, Auth services)
builder.Services.AddInfrastructure(builder.Configuration);

// Application services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();

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
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
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
builder.Services.AddAuthorization();

// Swagger config
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // 1. Garantir instâncias dos componentes de segurança
        document.Components ??= new Microsoft.OpenApi.Models.OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, Microsoft.OpenApi.Models.OpenApiSecurityScheme>();
        
        // 2. Define o esquema do JWT Bearer
        var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header usando o esquema Bearer. Exemplo: 'Bearer {token}'"
        };

        document.Components.SecuritySchemes.Add("Bearer", securityScheme);

        // 3. Criar o requisito que referencia o "Bearer" definido acima
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

        // 4. Aplicar o requisito de segurança em TODAS as rotas/operações da API de forma nativa
        if (document.Paths != null)
        {
            foreach (var path in document.Paths.Values)
            {
                foreach (var operation in path.Operations.Values)
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
        policy.WithOrigins("http://localhost:3000", "https://seu-frontend.com")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

//pipeline http request

// Ative para garantir tráfego criptografado
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