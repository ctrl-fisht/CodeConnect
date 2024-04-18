using AutoMapper;
using CodeConnect.CommonDto;
using CodeConnect.Entities;
using CodeConnect.Features.Activities;
using CodeConnect.Features.Activities.ActivityUsers;
using CodeConnect.Features.Communities;

namespace CodeConnect.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateCommunityInput, Community>();

        CreateMap<Community, CommunityDto>();

        CreateMap<Activity, ActivityDto>();

        CreateMap<City, CityDto>();

        CreateMap<ActivityCategory, ActivityCategoryDto>();
        CreateMap<Category, CategoryDto>();

        CreateMap<ActivityTag, ActivityTagDto>();
        CreateMap<Tag, TagDto>();

        CreateMap<CreateActivityInput, Activity>();
    }
}
