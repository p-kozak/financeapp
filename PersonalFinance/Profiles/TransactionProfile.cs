using AutoMapper;
using PersonalFinance.Domain;
using PersonalFinance.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
