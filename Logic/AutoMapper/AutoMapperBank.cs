using AutoMapper;
using Data.DataObjects;
using Logic.DataTransferObjects;
using Logic.DataTransferObjects.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.AutoMapper
{
    public class AutoMapperBank : Profile
    {
        public AutoMapperBank()
        {
            CreateMap<CreateUpdateBankDto, Bank>()
               .ForMember(c => c.Address, c => c.MapFrom(createUpdateBankDto => new Address()
               {
                   Country = createUpdateBankDto.Country,
                   City = createUpdateBankDto.City,
                   Street = createUpdateBankDto.Street,
                   HouseNumber = createUpdateBankDto.HouseNumber,
                   ContactInfo = new ContactInfo()
                   {
                       Email = createUpdateBankDto.Email,
                       PhoneNumber = createUpdateBankDto.PhoneNumber   
                   }


               })).ReverseMap();

        }   
            
    }
}
