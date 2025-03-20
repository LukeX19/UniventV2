using AutoMapper;
using Univent.App.Users.Dtos;
using Univent.Domain.Models.Users;

namespace Univent.App.MappingProfiles
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<AppUser, UserBasicInfoResponseDto>();
            CreateMap<AppUser, UserProfileResponseDto>()
                .ForMember(dest => dest.UniversityName, opt => opt.MapFrom(src => src.University.Name));
        }
    }
}
