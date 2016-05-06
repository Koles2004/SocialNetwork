using AutoMapper;
using SocialNetworkWithIdentity.Models;
using SocialNetworkWithIdentity.Models.DomainModels;
using SocialNetworkWithIdentity.Models.ViewModels;

namespace SocialNetworkWithIdentity
{
    public class AutoMapperConfig
    {
        // регистрируем мапперы
        public static void RegisterMappings()
        {
            Mapper.CreateMap<ApplicationUser, PeopleViewModel>(); // из ApplicationUser в PeopleViewModel
			Mapper.CreateMap<ApplicationUser, FirstMessageViewModel>();
			Mapper.CreateMap<FirstMessageViewModel, Message>();
            Mapper.CreateMap<PeopleViewModel, ApplicationUser>();
            Mapper.CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember("Gender", opt => opt.MapFrom(u => (u.Gender == "Male")))
                .ForMember("UserName", opt => opt.MapFrom(u => u.Email))
                .ForMember("Email", opt => opt.MapFrom(u => u.Email));
            Mapper.CreateMap<ApplicationUser, UserPageViewModel>()
                .ForMember("Gender", opt => opt.MapFrom(u => (u.Gender == true) ? "Мужской" : "Женский"));
            Mapper.CreateMap<UserPageViewModel, ApplicationUser>()
                .ForMember("Gender", opt => opt.MapFrom(u => u.Gender == "Мужской"));
            Mapper.CreateMap<Event, EventViewModel>();
        }
    }
}