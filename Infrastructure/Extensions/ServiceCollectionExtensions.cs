using Infrastructure.Authorization;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Add Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            


            //Add Service
            services.AddScoped<UserService>();
            services.AddScoped<PermissionService>();
            // Add Controllers, Swagger, Endpoints
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập token dạng: Bearer {token}"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration["Jwt:Key"];
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = issuer ?? "your-app",
                        ValidAudience = audience ?? "your-app",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? "default_key_with_32_characters"))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Headers["Authorization"].FirstOrDefault();
                            if (!string.IsNullOrEmpty(token))
                            {
                                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                                    context.Token = token.Substring("Bearer ".Length).Trim();
                                else
                                    context.Token = token;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }

        public static async Task AddDynamicAuthorizationAsync(this IServiceCollection services, IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var functions = await db.Functions.Select(f => f.Key).ToListAsync();

            services.AddAuthorization(options =>
            {
                foreach (var key in functions)
                {
                    options.AddPolicy($"Function:{key}", policy =>
                        policy.Requirements.Add(new FunctionRequirement(key)));
                }
            });
        }
    }
}
