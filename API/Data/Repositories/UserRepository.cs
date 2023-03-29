using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  public class UserRepository : IUserRepository
  {
    private readonly DataContext context;
    private readonly IMapper mapper;
    public UserRepository(DataContext context, IMapper mapper)
    {
      this.mapper = mapper;
      this.context = context;
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
      return await this.context.Users
        .Include(p => p.Photos)
        .ToListAsync();
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
      return await this.context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
      return await this.context.Users
        .Include(p => p.Photos)
        .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<bool> SaveAllAsync()
    {
      return await this.context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
      // Notifies the entity tracker that something has changed with our inputted user
      this.context.Entry(user).State = EntityState.Modified;
    }

    public async Task<IEnumerable<MemberDto>> GetMembersAsync()
    {
      return await this.context.Users
        .ProjectTo<MemberDto>(this.mapper.ConfigurationProvider)
        .ToListAsync();
    }

    public async Task<MemberDto> GetMemberAsync(string username)
    {
      return await this.context.Users
        .Where(x => x.UserName == username)
        .ProjectTo<MemberDto>(this.mapper.ConfigurationProvider)
        .SingleOrDefaultAsync();
    }
  }
}