using DesafioOriginSW_API.Models.Responses;
using FluentResults;

namespace DesafioOriginSW_API.Handlers.IHandler
{
    public interface ICardStateHandler
    {
        Task<Result<IEnumerable<CardStateResponse>>> GetAllCardStates();

        Task<Result<CardStateResponse>> GetCardState(int id);
    }
}
