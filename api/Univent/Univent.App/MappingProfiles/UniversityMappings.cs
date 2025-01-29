using AutoMapper;
using Univent.App.Universities.Dtos;
using Univent.Domain.Models.Universities;

namespace Univent.App.MappingProfiles
{
    public class UniversityMappings : Profile
    {
        public UniversityMappings()
        {
            CreateMap<University, UniversityResponseDto>();
        }
    }
}
