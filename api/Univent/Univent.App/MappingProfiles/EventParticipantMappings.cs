using AutoMapper;
using Univent.App.EventParticipants.Dtos;
using Univent.Domain.Models.Associations;

namespace Univent.App.MappingProfiles
{
    public class EventParticipantMappings : Profile
    {
        public EventParticipantMappings()
        {
            CreateMap<EventParticipant, EventParticipantFullResponseDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.User.PictureUrl));
        }
    }
}
