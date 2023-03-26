using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
  public static class IdentityServiceExtensions
  {
    public static IServiceCollection AddIdentityServices(
      this IServiceCollection services, IConfiguration config)
    {
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
          options.TokenValidationParameters = new TokenValidationParameters
          {
            // Requires issuer signing key
            ValidateIssuerSigningKey = true,
            // Specifies the issuer singing key
            IssuerSigningKey = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(config["TokenKey"])
            ),
            ValidateIssuer = false,
            ValidateAudience = false
          };
        }
      );

      return services;
    }
  }
}