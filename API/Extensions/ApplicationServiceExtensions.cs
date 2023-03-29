using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
  public static class ApplicationServiceExtensions
  {
    public static IServiceCollection AddApplicationServices(
      this IServiceCollection services, IConfiguration config)
    {
      services.AddDbContext<DataContext>(options => 
      {
        options.UseSqlite(config.GetConnectionString("DefaultConnection"));
      });
      services.AddSwaggerGen();

      services.AddCors();

      // These services are now injectable into our controllers
      services.AddScoped<ITokenService, TokenService>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

      return services;
    }
  }
}