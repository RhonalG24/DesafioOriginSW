using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Responses.CardState;
using FluentResults;

namespace DesafioOriginSW_API.Handlers.IHandler
{
    public interface ICardStateHandler
    {
        Task<Result<GetAllCardStatesResponse>> GetAllCardStates();

        Task<Result<GetCardStateResponse>> GetCardState(int id);
    }
}
