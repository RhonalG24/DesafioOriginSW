using AutoMapper;
using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Models.Errors;
using DesafioOriginSW_API.Models.Responses;
using DesafioOriginSW_API.Repository.IRepository;
using FluentResults;


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

        public async Task<Result<IEnumerable<CardStateResponse>>> GetAllCardStates()
        {
            IEnumerable<CardStateResponse> cardStateList = _mapper.Map<IEnumerable<CardStateResponse>>(await _cardStateRepository.GetAll());
            if (cardStateList == null || cardStateList.Count() == 0)
                return new NotFoundError("No se encontró ningún registro");
            return Result.Ok(cardStateList);
        }

        public async Task<Result<CardStateResponse>> GetCardState(int id)
        {
            if (id < 0)
                return new RequestValidationError("El id ingresado no es válido.");

            var result = await GetCardStateById(id);
            if (result == null)
                return new NotFoundError("Id no encontrado.");

            return Result.Ok(result);

        }

        private async Task<CardStateResponse> GetCardStateById( int id )
        {
            var result = await _cardStateRepository.Get(x => x.id_card_state == id);
            return _mapper.Map<CardStateResponse>(result);
        }
    }
}
