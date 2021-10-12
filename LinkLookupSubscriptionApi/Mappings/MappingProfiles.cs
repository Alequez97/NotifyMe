using AutoMapper;
using LinkLookupSubscriptionApi.Models;
using LinkLookupSubscriptionApi.Models.DTO;
using MessageSender.Models;

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