using AutoMapper;
using CodeConnect.Dto;
using CodeConnect.Entities;
using CodeConnect.Features.Activities;
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

        CreateMap<ActivityImage, ActivityImageDto>();
        CreateMap<CommunityImage, CommunityImageDto>();
    }
}
