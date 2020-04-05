using AutoMapper;
using PersonalFinance.Domain;
using PersonalFinance.Domain.DTOs;

namespace PersonalFinance.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<TransactionDTO, Transaction>();
        }
    }
}
