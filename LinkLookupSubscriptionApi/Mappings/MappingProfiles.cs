using AutoMapper;
using LinkLookupBackgroundService.Configuration.Models;
using LinkLookupSubscriptionApi.Models;
using LinkLookupSubscriptionApi.Models.DTO;

namespace LinkLookupSubscriptionApi.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<NotifyConfig, NotifyConfigDto>();
            CreateMap<NotifyConfigDto, NotifyConfig>();
        }
    }
}