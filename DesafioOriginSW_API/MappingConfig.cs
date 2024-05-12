using AutoMapper;
using DesafioOriginSW_API.DTO_s;
using DesafioOriginSW_API.Models;

namespace DesafioOriginSW_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Account, AccountDTO>().ReverseMap();
            CreateMap<Account, UpdateAccountDTO>().ReverseMap();

            CreateMap<BankCard, BankCardDTO>().ReverseMap();
            CreateMap<BankCard, BankCardPinDTO>().ReverseMap();
            CreateMap<BankCard, CreateBankCardDTO>().ReverseMap();

            CreateMap<Operation, OperationDTO>().ReverseMap();
            CreateMap<Operation, CreateOperationDTO>().ReverseMap();

        }
    }
}
