using AutoMapper;
using Univent.App.EventTypes.Dtos;
using Univent.Domain.Models.Events;

namespace Univent.App.MappingProfiles
{
    public class EventTypeMappings : Profile
    {
        public EventTypeMappings()
        {
            CreateMap<EventType, EventTypeResponseDto>();
        }
    }
}
