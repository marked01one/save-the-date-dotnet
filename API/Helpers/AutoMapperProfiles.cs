using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<AppUser, MemberDto>()
        .ForMember(dest => dest.PhotoUrl, 
          option => option.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
        .ForMember(dest => dest.Age, 
          option => option.MapFrom(src => src.DateOfBirth.CalculateAge()));
      CreateMap<Photo, PhotoDto>();
    }
  }
}