using AutoMapper;
using Azure.Core;
using DesafioOriginSW_API.DTO_s;
using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Entities;
using DesafioOriginSW_API.Models.Errors;
using DesafioOriginSW_API.Models.Requests.BankCard;
using DesafioOriginSW_API.Models.Responses.BankCard;
using DesafioOriginSW_API.Repository;
using DesafioOriginSW_API.Repository.IRepository;
using FluentResults;
using System.Net;

namespace DesafioOriginSW_API.Handlers
{
    public class BankCardHandler : IBankCardHandler
    {
        private readonly IBankCardRepository _bankCardRepository;
        private readonly ICardStateRepository _cardStateRepository;
        private readonly IAccountHandler _accountHandler;
        private readonly IMapper _mapper;

        public BankCardHandler(
            IBankCardRepository bankCardRepository,
            ICardStateRepository cardStateRepository,
            IAccountHandler accountHandler,
            IMapper mapper )
        {
            _cardStateRepository = cardStateRepository ?? throw new ArgumentNullException(nameof(cardStateRepository));
            _bankCardRepository = bankCardRepository ?? throw new ArgumentNullException(nameof(bankCardRepository));
            _accountHandler = accountHandler ?? throw new ArgumentNullException(nameof(accountHandler));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<GetAllBankCardResponse>> GetAllBankCards()
        {

            IEnumerable<BankCard> bankCardList = await _bankCardRepository.GetAll();
            if (bankCardList == null)
                return new NotFoundError("No se encontraron registros");

            var response = _mapper.Map<GetAllBankCardResponse>(bankCardList);

            return Result.Ok(response);
            

        }

        public async Task<Result<GetBankCardResponse>> GetBankCard(int id)
        {
            if (id <= 0)
                return new RequestValidationError("El id solicitado no es válido.");

            var bankCardFiltered = await GetBankCardById(id);
            if (bankCardFiltered == null)
                return new NotFoundError("El id no existe");

            var response = _mapper.Map<GetBankCardResponse>(bankCardFiltered);
            return Result.Ok(response);
        }

        public async Task<Result<CreateBankCardResponse>> CreateBankCard(CreateBankCardRequest request)
        {
            //verify that associated account exist
            if (await _accountHandler.GetAccountById(request.id_account) == null)
                return new RequestValidationError("El Id de cliente no existe.");

            BankCard newBankCard = _mapper.Map<BankCard>(request);
            newBankCard.expiry_date = DateOnly.FromDateTime(DateTime.Now.AddYears(10));
            newBankCard.id_card_state = (int)BankCardState.activa; //Active

            await _bankCardRepository.Create(newBankCard);

            var response = _mapper.Map<CreateBankCardResponse>(newBankCard);
            
            return Result.Ok(response);

        }

        public async Task<Result<CheckBankCardNumberResponse>> CheckBankCardNumber(CheckBankCardNumberRequest request)
        {
            //BankCard bankCardFiltered = await GetBankCardByCardNumber(request.bank_card_number) ?? throw new ArgumentNullException(nameof(bankCardFiltered));
            var bankCardFiltered = await GetBankCardByCardNumber(request.bank_card_number);
            if (bankCardFiltered == null)
                return new NotFoundError("No existe el id de la cuenta");

            //Verify if it's blocked
            if (bankCardFiltered.id_card_state == (int)BankCardState.bloqueada)
                return new UnauthorizedError("La tarjeta está bloqueada");
            //return new UnauthorizedError("The card is blocked");

            var response = _mapper.Map<CheckBankCardNumberResponse>(bankCardFiltered);

            return Result.Ok(response);
        }

        public async Task<Result<CheckBankCardPinResponse>> CheckBankCardPin(CheckBankCardPinRequest request)
        {
            throw new NotImplementedException();
        }


        private async Task<BankCard> GetBankCardById(int id)
{
            return await _bankCardRepository.Get(v => v.id_bank_card == id);
        }        

        private async Task<BankCard> GetBankCardByCardNumber(string cardNumber)
        {
            return await _bankCardRepository.Get(v => v.number == cardNumber);
        }

        private async Task<int> GetIdForBlockedCardState()
        {
            CardState cardState = await _cardStateRepository.Get(v => v.name == "bloqueada" );

            return cardState.id_card_state;
        }

        private int GetMaxFailedAttempts()
        {
            return Int32.Parse(Environment.GetEnvironmentVariable("MAX_PIN_FAILED_ATTEMPTS") ?? "4");
        }
    }
}
