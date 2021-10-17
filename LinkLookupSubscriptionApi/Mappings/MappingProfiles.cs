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
            CreateMap<Group, GroupDto>();
            CreateMap<GroupDto, Group>();
            CreateMap<NotifyConfig, NotifyConfigDto>();
            CreateMap<NotifyConfigDto, NotifyConfig>();
        }
    }
}