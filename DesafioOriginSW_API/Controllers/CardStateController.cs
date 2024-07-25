using AutoMapper;
using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Errors;
using DesafioOriginSW_API.Models.Responses.CardState;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DesafioOriginSW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardStateController : ResultsControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICardStateHandler _handler;
        private readonly IMapper _mapper;

        public CardStateController(
            ILogger<CardStateController> logger,
            ICardStateHandler handler,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region Decorators [HttpGet]
        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<GetAllCardStatesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<GetAllCardStatesResponse>> GetAllCardStates()
        {
            var cardStateList = await _handler.GetAllCardStates();

            return Ok(cardStateList);

        }

        #region [HttpGet("{id}", Name = "GetCardState")]
        [HttpGet("{id}", Name = "GetCardState")]
        [ProducesResponseType(typeof(APIResponse<GetCardStateResponse>) ,StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<GetCardStateResponse>> GetCardState(int id)
        { 
            var cardState = await _handler.GetCardState(id);

            return Ok(cardState);
        }
    }
}
