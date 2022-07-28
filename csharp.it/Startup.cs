using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using csharp.it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using csharp.it.Mappings;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using csharp.it.Controllers;

namespace csharp.it
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
            services.AddControllers();

            services.AddDbContext<Models.DbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddCors();

            // ---------- START OF SERVICES -----------
            services.AddTransient<Seeder>();
            // ---------- END OF SERVICES -----------

            services.AddHttpClient();

            // ---------- START OF IDENTITY -----------
            services.AddIdentity<User, Role>().AddEntityFrameworkStores<Models.DbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
            });
            // ---------- END OF IDENTITY -----------

            // ---------- START OF MAPPER ----------
            var config = new AutoMapper.MapperConfiguration(c =>
            {
                c.AddProfile(new ApplicationMapping());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            // ---------- END OF MAPPER -----------

            // ---------- START OF JWT AUTHENTICATION -----------
            services.Configure<TokenSettings>(Configuration.GetSection("Jwt"));
            services.AddOptions<TokenSettings>("Jwt");

            services.AddAuthentication(cfg =>
                    {
                        cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configuration["Jwt:Issuer"].ToString(),
                            ValidateAudience = true,
                            ValidAudience = Configuration["Jwt:Audience"].ToString(),
                            ValidateLifetime = true,

                            IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                                Configuration["Jwt:Key"])),
                            ValidateIssuerSigningKey = true,
                        };
                    });
            // ---------- START OF JWT AUTHENTICATION -----------

            // ---------- START OF SWAGGER -----------
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
            // ---------- END OF SWAGGER -----------
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Seeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            seeder.Seed().Wait();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
