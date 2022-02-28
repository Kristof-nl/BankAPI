using AutoMapper;
using Data.DataObjects;
using Logic.DataTransferObjects;
using Logic.DataTransferObjects.Bank;
using Logic.DataTransferObjects.BankTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.AutoMapper
{
    public class AutoMapperBankTransaction : Profile
    {
        public AutoMapperBankTransaction()
        {
            CreateMap<BankTransaction, BankTransactionDto>().ReverseMap();
            CreateMap<ShortBankTransactionDto, BankTransactionDto>().ReverseMap();
            CreateMap<BankTransaction, ShortBankTransactionDto>().ReverseMap();

        }   
            
    }
}
