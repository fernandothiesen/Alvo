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
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder();



// Services configurations 


//Infrastructure (DB, Repositories, Auth services)
builder.Services.AddInfrastructure(builder.Configuration);

//Application services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();


//JWt && Auth configurations

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


    //receives token by string query (good for swagger)
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if(!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});


//Authorization config

builder.Services.AddAuthorization();

// Swagger config


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema Alvo API",
        Version = "v1",
        Description = "API for managing events, costumers, demandas, supplier",
        Contract = new OpenApiContact
        {
            Name = "Fernando Thiesen Dalla Valle",
            Email = "fernandothiesen17@gmail.com"
        }
    });


    //auth config 
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
         Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });


    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


//controllers config 

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //format data ISO standards 
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true;
    });

//Cors config

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    // Política mais restritiva (recomendada para produção)
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://seu-frontend.com")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();



//pipeline config http


//enable swagger development only 
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndPoint("/swagger/v1/swagger.json", "Sistema de Eventos API v1");
        c.RoutePrefix = "swagger";
    });
}


//authentication && authorization Middleware 

app.UseAuthentication();
app.UseAuthorization();


//Cors  
app.UseCors("AllowAll");  // Use "AllowFrontend" em produção


app.MapControllers();



app.Run();

