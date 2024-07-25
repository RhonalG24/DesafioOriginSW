using AutoMapper;
using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Handlers.Shared;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Entities;
using DesafioOriginSW_API.Models.Errors;
using DesafioOriginSW_API.Models.Responses.CardState;
using DesafioOriginSW_API.Repository.IRepository;
using FluentResults;
using System.Net;


namespace DesafioOriginSW_API.Handlers
{
    public class CardStateHandler : ICardStateHandler
    {
        private ICardStateRepository _cardStateRepository;
        private IMapper _mapper;

        public CardStateHandler(ICardStateRepository cardStateRepository, IMapper mapper)
        {
            _cardStateRepository = cardStateRepository ?? throw new ArgumentNullException(nameof(cardStateRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<GetAllCardStatesResponse>> GetAllCardStates()
        {
            IEnumerable<GetCardStateResponse> cardStateList = _mapper.Map<IEnumerable<GetCardStateResponse>>(await _cardStateRepository.GetAll());
            if (cardStateList == null || cardStateList.Count() == 0)
                return new NotFoundError("No se encontró ningún registro");

            var response = new GetAllCardStatesResponse();
            response.CardStates = cardStateList;

            return Result.Ok(response);
        }

        public async Task<Result<GetCardStateResponse>> GetCardState(int id)
        {
            if (id < 0)
                return new RequestValidationError("El id ingresado no es válido.");

            var cardState = await GetCardStateById(id);
            if (cardState == null)
                return new NotFoundError("Id no encontrado.");

            var response = _mapper.Map<GetCardStateResponse>(cardState);

            return Result.Ok(response);
        }

        private async Task<CardState> GetCardStateById( int id )
        {
            var result = await _cardStateRepository.Get(x => x.id_card_state == id);
            return result;
        }
    }
}
