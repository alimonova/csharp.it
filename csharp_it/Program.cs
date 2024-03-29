﻿using System.Text;
using csharp_it.Controllers;
using csharp_it.Mappings;
using csharp_it.Models;
using csharp_it.Services;
using csharp_it.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<csharp_it.Models.DbContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors();

// ---------- START OF builder.Services -----------
builder.Services.AddTransient<Seeder>();
builder.Services.AddScoped<IAccessService, AccessService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IChapterService, ChapterService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITarifService, TarifService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IPracticalExampleService, PracticalExampleService>();
builder.Services.AddScoped<IUsefulResourceService, UsefulResourceService>();
builder.Services.TryAddScoped<IHttpContextAccessor, HttpContextAccessor>();
// ---------- END OF builder.Services -----------

// ---------- START OF Services for Email sender -----------
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
// ---------- END OF Services for Email sender -----------

builder.Services.AddHttpClient();

// ---------- START OF IDENTITY -----------
builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<csharp_it.Models.DbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
});
// ---------- END OF IDENTITY -----------

// ---------- START OF MAPPER ----------
var config = new AutoMapper.MapperConfiguration(c =>
{
    c.AddProfile(new ApplicationMapping());
});

var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
// ---------- END OF MAPPER -----------

builder.Services.Configure<WayForPaySettings>(builder.Configuration.GetSection("WayForPay"));
builder.Services.AddOptions<WayForPaySettings>("WayForPay");

// ---------- START OF JWT AUTHENTICATION -----------
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddOptions<TokenSettings>("Jwt");

builder.Services.AddAuthentication(cfg =>
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
                ValidIssuer = builder.Configuration["Jwt:Issuer"].ToString(),
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"].ToString(),
                ValidateLifetime = true,

                IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                    builder.Configuration["Jwt:Key"])),
                ValidateIssuerSigningKey = true,
            };
        });
// ---------- START OF JWT AUTHENTICATION -----------

// ---------- START OF SWAGGER -----------
builder.Services.AddSwaggerGen(c =>
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:4200")
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod();
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

SeedDatabase(); 
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();

void SeedDatabase() //can be placed at the very bottom under app.Run()
{
    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
        seeder.Seed().Wait();
    }
}