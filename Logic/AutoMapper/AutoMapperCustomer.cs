using AutoMapper;
using Data.DataObjects;
using Logic.DataTransferObjects.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.AutoMapper
{
    public class AutoMapperCustomer : Profile
    {
        public AutoMapperCustomer()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<ShortCustomerDto, CustomerDto>().ReverseMap();
            CreateMap<Customer, ShortCustomerDto>().ReverseMap();
            CreateMap<Customer, ShortCustomerDtoWithTransaction>().ReverseMap();
            CreateMap<CreateCustomerDto, Customer>()
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
