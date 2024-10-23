using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Auth
{
    public static class SettingsAuthService
    {
        private static IConfiguration Configuration { get; }
        static SettingsAuthService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }
        public static string SecretKey => Configuration["Values:SecretKey"];

        public static string ExpirationHours => Configuration["Values:ExpirationHours"];


        public static string GenerateJwtToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(SecretKey);
            var symmetricKey = new SymmetricSecurityKey(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("name", user.Nome),
                    new Claim("email", user.Email),

                }),
                Expires = DateTime.UtcNow.AddHours(double.Parse(ExpirationHours)),
                SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static UserContext ValidateJwtToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKeyEncoding = Encoding.ASCII.GetBytes(SecretKey);


                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyEncoding),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var user = new Usuario
                {
                    Id = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value),
                    Nome = jwtToken.Claims.First(x => x.Type == "name").Value,
                    Email = jwtToken.Claims.First(x => x.Type == "email").Value,
                };

                var userContext = new UserContext
                {
                    User = user,
                    Claims = jwtToken.Claims
                };

                return userContext;
            }
            catch (Exception ex)
            {
                throw new Exception("You aren't authenticated! Error - " + ex);
            }
        }
    }
}
