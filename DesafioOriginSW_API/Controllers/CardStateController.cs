using AutoMapper;
using DesafioOriginSW_API.Data;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DesafioOriginSW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardStateController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICardStateRepository _repo;
        private readonly IMapper _mapper;
        protected APIResponse _response;


        public CardStateController(ILogger<CardStateController> logger,
                                    ICardStateRepository repo,
                                    IMapper mapper)
        {
            _logger = logger;
            _repo = repo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllCardStates()
        {
            try
            {
                IEnumerable<CardState> cardStateList = await _repo.GetAll();
                _response.Result = cardStateList;
                _response.IsSuccessful = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            } catch (Exception ex)
            {
                _logger.LogError("GetAllCardStates", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() }; 
                return _response;
            }

        }

        [HttpGet("id:int", Name = "GetCardState")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCardState(int id)
        {
            try
            {
                //_logger.LogInformation("Get all accounts");
                var cardStateFiltered = await _repo.Get(x => x.id_card_state == id);
                if (cardStateFiltered == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);

                }
                _response.Result = cardStateFiltered;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get operation", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }

        }
    }
}
