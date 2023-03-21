using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
  public class AccountController : BaseApiController
  {
    public DataContext context { get; }
    public AccountController(DataContext context)
    {
      this.context = context;
    }

    [HttpPost("register")]  // POST: api/account/register
    public async Task<ActionResult<AppUser>> Register(RegisterDto requestBody)
    {
      // Checks if user already exists in database
      if (await UserExists(requestBody.Username)) return BadRequest("Username is already taken");

      using var hmac = new HMACSHA512();

      var user = new AppUser
      {
        UserName = requestBody.Username.ToLower(),
        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(
          requestBody.Password
        )),
        PasswordSalt = hmac.Key
      };

      this.context.Users.Add(user);
      await this.context.SaveChangesAsync();
      
      return user;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AppUser>> Login(LoginDto requestBody)
    {
      var user = await this.context.Users.SingleOrDefaultAsync(
        user => user.UserName == requestBody.Username.ToLower()
      );

      if (user == null) return Unauthorized();

      using var hmac = new HMACSHA512(user.PasswordSalt);

      var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(requestBody.Password));

      // Validates whether given password correlates to the given user
      for (int i = 0; i < computedHash.Length; i++)
      {
        if (computedHash[i] != user.PasswordHash[i]) return Unauthorized();
      }

      return Ok(user);
    }

    private async Task<bool> UserExists(string username)
    {
      // if user exists --> return true
      // if user doesn't exists --> return false
      return await this.context.Users.AnyAsync(user => user.UserName == username.ToLower());
    }


  }
}