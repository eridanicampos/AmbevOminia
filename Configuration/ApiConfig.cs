using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectTest.Middleware;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System.Security.Claims;
using ProjectTest.Application.Auth;

namespace ProjectTest.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options => options.AddPolicy(name: "localhost", policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader();
                policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
                policy.WithOrigins("https://localhost:44380").AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddSwaggerConfig();

            // Configurar JWT Authentication
            var key = Encoding.ASCII.GetBytes(configuration["Values:SecretKey"]);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                // Adicionando um evento de validação de token
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        // Converta o token para JwtSecurityToken
                        var jwtToken = context.SecurityToken as JwtSecurityToken;
                        if (jwtToken != null)
                        {
                            var userContext = SettingsAuthService.ValidateJwtToken(jwtToken.RawData);
                            // Adicionar as claims personalizadas ao contexto
                            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                            if (claimsIdentity != null)
                            {
                                claimsIdentity.AddClaims(userContext.Claims);
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ProjectTest API", Version = "v1" });
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            return services;
        }

        public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors("localhost");
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();
            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "DefaultApi",
                    pattern: "api/{controller}/{id?}");
            });

            return app;
        }
    }
}
