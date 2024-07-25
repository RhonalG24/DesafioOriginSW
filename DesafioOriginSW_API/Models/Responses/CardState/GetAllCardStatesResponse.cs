using DesafioOriginSW_API.Models.Entities;

namespace DesafioOriginSW_API.Models.Responses.CardState
{
    public class GetAllCardStatesResponse
    {
        public IEnumerable<GetCardStateResponse> CardStates { get; set; }
    }
}
