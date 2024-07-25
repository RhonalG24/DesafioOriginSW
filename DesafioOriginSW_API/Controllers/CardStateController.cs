using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DesafioOriginSW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardStateController : ResultsControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICardStateHandler _handler;

        public CardStateController(
            ILogger<CardStateController> logger,
            ICardStateHandler handler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));

        }

        #region Decorators [HttpGet]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CardStateResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<IEnumerable<CardStateResponse>>> GetAllCardStates()
        {
            var cardStateList = await _handler.GetAllCardStates();

            return Ok(cardStateList);

        }

        #region [HttpGet("{id}", Name = "GetCardState")]
        [HttpGet("{id}", Name = "GetCardState")]
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<CardStateResponse>> GetCardState(int id)
        { 
            var cardState = await _handler.GetCardState(id);
            return Ok(cardState);
        }
    }
}
