using AutoMapper;
using Univent.App.Events.Dtos;
using Univent.Domain.Models.Events;

namespace Univent.App.MappingProfiles
{
    public class EventMappings : Profile
    {
        public EventMappings()
        {
            CreateMap<Event, EventSummaryResponseDto>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Name))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => new EventAuthorResponseDto
                {
                    Id = src.Author.Id,
                    FirstName = src.Author.FirstName,
                    LastName = src.Author.LastName,
                    PictureUrl = src.Author.PictureUrl,
                    Rating = 0.0
                }));

            CreateMap<Event, EventFullResponseDto>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Name))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => new EventAuthorResponseDto
                {
                    Id = src.Author.Id,
                    FirstName = src.Author.FirstName,
                    LastName = src.Author.LastName,
                    PictureUrl = src.Author.PictureUrl,
                    Rating = 0.0
                }));
        }
    }
}
