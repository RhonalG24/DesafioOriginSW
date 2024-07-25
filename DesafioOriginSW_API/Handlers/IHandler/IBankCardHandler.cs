using DesafioOriginSW_API.DTO_s;
using DesafioOriginSW_API.Models.Requests.BankCard;
using DesafioOriginSW_API.Models.Responses.BankCard;
using FluentResults;

namespace DesafioOriginSW_API.Handlers.IHandler
{
    public interface IBankCardHandler
    {

        Task<Result<GetAllBankCardResponse>> GetAllBankCards();
        Task<Result<GetBankCardResponse>> GetBankCard(int id);
        Task<Result<CreateBankCardResponse>> CreateBankCard(CreateBankCardRequest request);
        Task<Result<CheckBankCardNumberResponse>> CheckBankCardNumber(CheckBankCardNumberRequest request);
        Task<Result<CheckBankCardPinResponse>> CheckBankCardPin(CheckBankCardPinRequest request);

    }
}
