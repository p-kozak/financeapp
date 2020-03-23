using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Domain.Profiles
{
    public  class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegistrationModel, User>();
        }
    }
}
