using AutoMapper;
using DesafioOriginSW_API.DTO_s;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Entities;
using DesafioOriginSW_API.Models.Responses.Account;
using DesafioOriginSW_API.Models.Responses.CardState;
using DesafioOriginSW_API.Models.Responses.OperationType;

namespace DesafioOriginSW_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Account, AccountDTO>().ReverseMap();
            CreateMap<Account, UpdateAccountDTO>().ReverseMap();
            CreateMap<Account, GetAllAccountsResponse>().ReverseMap();
            CreateMap<Account, GetAccountResponse>().ReverseMap();
            CreateMap<Account, UpdateAccountResponse>().ReverseMap();
            CreateMap<Account, UpdatePartialAccountResponse>().ReverseMap();

            CreateMap<CardState, GetCardStateResponse>().ReverseMap();

            CreateMap<BankCard, BankCardDTO>().ReverseMap();
            CreateMap<BankCard, BankCardPinDTO>().ReverseMap();
            CreateMap<BankCard, CreateBankCardDTO>().ReverseMap();
            CreateMap<BankCard, CheckBankCardNumberRespondDTO>().ReverseMap();


            CreateMap<Operation, OperationDTO>().ReverseMap();
            CreateMap<Operation, CreateOperationDTO>().ReverseMap();


            CreateMap<OperationType, GetOperationTypeResponse>().ReverseMap();

        }
    }
}
