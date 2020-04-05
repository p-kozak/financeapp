using AutoMapper;

namespace PersonalFinance.Domain.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegistrationModel, User>();
        }
    }
}
