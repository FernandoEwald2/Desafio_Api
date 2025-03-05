using Desafio.Data;
using Desafio.Services;
using Desafio.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using Desafio.Api.Services.Interfaces;
using Desafio.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Desafio
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuração da string de conexão com variáveis de ambiente
            var config = Configuration.GetConnectionString("DefaultConnection");
            var Audience = Configuration["TokenSetting:Audience"];
            var Issuer = Configuration["TokenSetting:Issuer"];
            

            var dbUser = Environment.GetEnvironmentVariable("DB_USER_DESAFIO");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD_DESAFIO");

            if (dbUser != null && dbPassword != null)
            {
                config = config.Replace("{DB_USER_DESAFIO}", dbUser)
                               .Replace("{DB_PASSWORD_DESAFIO}", dbPassword);
            }

            // Configuração do DbContext
            services.AddDbContext<DesafioDbContext>(options =>
                options.UseSqlServer(config)
                       .EnableSensitiveDataLogging(false));

            // Registro de serviços
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ITarefaService, TarefaService>();
            services.AddScoped<IAuthService, AuthService>();

            // Adiciona autenticação JWT
            var jwtSecretKey = Encoding.ASCII.GetBytes( Environment.GetEnvironmentVariable("WT_SECRET"));



            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => 
                    {
                        x.RequireHttpsMetadata = false;
                        x.SaveToken = true;
                        x.TokenValidationParameters = new TokenValidationParameters 
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(jwtSecretKey),
                                ValidateIssuer = false,
                                ValidateAudience = false
                            };

                    });
           

            services.AddAuthorization();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials());
            });

            // Configuração dos controladores e JSON
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null); // Garante que o nome das propriedades será como no modelo

            // Configuração do Swagger           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Desafio", Version = "v1" });

                // Definir o esquema de segurança para o Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Bearer ",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });                

                // Definir as operações que exigem autenticação
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
                            new string[] {}
                        }
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Desafio v1");
                    c.RoutePrefix = string.Empty; // Tornar o Swagger acessível na raiz da API
                });
            }

            // Habilita o roteamento de requisições
            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            // Habilita a autenticação e autorização
            app.UseAuthentication(); // Coloque antes de UseAuthorization
            app.UseAuthorization();   // Depois da autenticação, habilite a autorização

            // Mapeia os controladores
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
