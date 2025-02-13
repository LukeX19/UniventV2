using AutoMapper;
using Univent.App.Users.Dtos;
using Univent.Domain.Models.Users;

namespace Univent.App.MappingProfiles
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<AppUser, UserResponseDto>();
        }
    }
}
