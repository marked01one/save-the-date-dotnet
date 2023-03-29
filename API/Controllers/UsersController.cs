using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  [Authorize]
  public class UsersController : BaseApiController
  {
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
      this.mapper = mapper;
      this.userRepository = userRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()  // ActionResult<> allows you to return standard HTTP responses (i.e. NotFound())
    {
      return Ok(await this.userRepository.GetMembersAsync());
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
      return await this.userRepository.GetMemberAsync(username);
    }
  }
}