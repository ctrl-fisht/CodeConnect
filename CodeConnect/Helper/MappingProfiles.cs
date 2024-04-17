using AutoMapper;
using CodeConnect.Entities;
using CodeConnect.Features.Communities;

namespace CodeConnect.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateCommunityInput, Community>();
        CreateMap<Community, CommunityDto>();
    }
}
