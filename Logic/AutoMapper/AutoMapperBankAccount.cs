using AutoMapper;
using Data.DataObjects;
using Logic.DataTransferObjects;
using Logic.DataTransferObjects.Bank;
using Logic.DataTransferObjects.BankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.AutoMapper
{
    public class AutoMapperBankAccount : Profile
    {
        public AutoMapperBankAccount()
        {
            CreateMap<BankAccount, BankAccountDto>().ReverseMap();
            CreateMap<BankAccount, CreateBankAccountDto>().ReverseMap();
            CreateMap<ShortBankAccountDto, BankAccountDto>().ReverseMap();
            CreateMap<BankAccount, ShortBankAccountDto>().ReverseMap();
            CreateMap<BankAccount, VeryShortBankAccountDto>().ReverseMap();
            CreateMap<VeryShortBankAccountDto, ShortBankAccountDto>().ReverseMap();
        }     
    }
}
    