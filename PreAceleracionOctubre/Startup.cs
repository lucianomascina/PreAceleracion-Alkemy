using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using PreAceleracionOctubre.Context;
using PreAceleracionOctubre.Entities;
using PreAceleracionOctubre.Interfaces;
using PreAceleracionOctubre.Repositories;
using PreAceleracionOctubre.Services;
using SendGrid.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreAceleracionOctubre
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PreAceleracionOctubre", Version = "v1" });

                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Ingrese Bearer Token para poder autenticarse dentro de la aplicacion"
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
                        new List<string>()
                    }
                });
            });

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UserContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = "https://localhost:5001",
                        ValidIssuer = "https://localhost:5001",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeySecretaSuperLargaDeAutorizacion"))
                    };
                });

            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<DisneyContext>((services, options) =>
            {
                options.UseInternalServiceProvider(services);
                options.UseSqlServer(Configuration.GetConnectionString("DisneyConnectionString"));
            });
            services.AddDbContext<UserContext>((services, options) =>
            {
                options.UseInternalServiceProvider(services);
                options.UseSqlServer(Configuration.GetConnectionString("UsersConnectionString"));
            });

            services.AddSingleton(Configuration);
            services.AddScoped<IPersonajeRepository, PersonajeRepository>();
            services.AddScoped<IPeliculaRepository, PeliculaRepository>();
            services.AddScoped<IGeneroRepository, GeneroRepository>();

            services.AddSendGrid(o =>
            {
                o.ApiKey = "";
            });

            services.AddSingleton<IMailService, MailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PreAceleracionOctubre v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
