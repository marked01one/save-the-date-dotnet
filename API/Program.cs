using System.Text;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors(builder => 
  builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200")
);

// Checks for valid token
app.UseAuthentication();
// Checks for what is allowed with the valid token
app.UseAuthorization();

app.MapControllers();

// Automatically apply migrations & update DB when server starts
using var scope = app.Services.CreateAsyncScope();
var services = scope.ServiceProvider;
try
{
  var context = services.GetRequiredService<DataContext>();
  await context.Database.MigrateAsync();
  await Seed.SeedUsers(context);
}
catch (Exception ex)
{
  var logger = services.GetService<ILogger<Program>>();
  logger.LogError(ex, "An error occured during migration");
}

app.Run();
