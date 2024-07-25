using AutoMapper;
using DesafioOriginSW_API.Data;
using DesafioOriginSW_API.DTO_s;
using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Entities;
using DesafioOriginSW_API.Models.Requests.BankCard;
using DesafioOriginSW_API.Models.Responses.BankCard;
using DesafioOriginSW_API.Repository.IRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DesafioOriginSW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankCardController : ResultsControllerBase
    {
        private readonly ILogger _logger;
        private readonly IBankCardRepository _repo;
        private readonly ICardStateRepository _repoCardState;
        private readonly IAccountRepository _repoAccount;
        private readonly IOperationTypeRepository _repoOperationType;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        private readonly IBankCardHandler _bankCardHandler;
        //protected APIResponse _response;


        public BankCardController(
            ILogger<OperationController> logger,
            IBankCardRepository repo,
            ICardStateRepository repoCardState,
            IAccountRepository repoAccount,
            IOperationTypeRepository repoOperationType,
            AppDbContext db,
            IMapper mapper
,
            IBankCardHandler bankCardHandler)
        {
            _logger = logger;
            _repo = repo;
            _repoCardState = repoCardState;
            _repoAccount = repoAccount;
            _repoOperationType = repoOperationType;
            _db = db;
            _mapper = mapper;
            _bankCardHandler = bankCardHandler ?? throw new ArgumentNullException(nameof(bankCardHandler));
            //_response = new();
        }


        #region [HttpGet]
        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<GetAllBankCardResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<GetAllBankCardResponse>> GetAllBankCards()
        {
            var response = await _bankCardHandler.GetAllBankCards();
            return Ok(response);

        }

        #region [HttpGet("{id}", Name = "GetBankCard")]
        [HttpGet("{id}", Name = "GetBankCard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<GetBankCardResponse>> GetBankCard(int id)
        {
            var response = await _bankCardHandler.GetBankCard(id);
            return Ok(response);

        }

        #region [HttpPost]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        #endregion
        public async Task<ActionResult<CreateBankCardResponse>> CreateBankCard([FromBody] CreateBankCardRequest request)
        {
            if (!IsModelStateValid(ModelState))
                return ReturnModelStateErrors(ModelState);

            var response = await _bankCardHandler.CreateBankCard(request);
            return Ok(response);
        }

        #region [HttpGet("check/number/{bank_card_number}", Name = "CheckBankCardNumber")]
        //[EnableCors("AllowSpecificOrigin")]
        [HttpGet("check/number/{bank_card_number}", Name = "CheckBankCardNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<CheckBankCardNumberResponse>> CheckBankCardNumber(CheckBankCardNumberRequest request)
        {
            if (!IsModelStateValid(ModelState))
                return ReturnModelStateErrors(ModelState);

            var response = await _bankCardHandler.CheckBankCardNumber(request);
            return Ok(response);

        }

        #region [HttpPost("check/pin/{bank_card_id}", Name = "CheckCardPin")]
        [HttpPost("check/pin/{bank_card_id}", Name = "CheckCardPin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<CheckBankCardPinResponse>> CheckBankCardPin(int bank_card_id, [FromBody] CheckBankCardPinRequest request)
        {
            if (!IsModelStateValid(ModelState))
                return ReturnModelStateErrors(ModelState);

            APIResponse<BankCard> _response = new();
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                
                BankCard bankCardFiltered = await _repo.Get(v => v.id_bank_card == request.id_bank_card);
                if (bankCardFiltered == null)
                {
                    List<string> errors = new List<string>{"not found", "no se encontró el dato" };
                    APIResponse<BankCard> apiResponse = new(result: null, isSuccessful: false, statusCode: HttpStatusCode.NotFound, errorsMessage: errors);

                    return NotFound(apiResponse);

                }
                //Verify if it's blocked
                if (bankCardFiltered.id_card_state == await GetIdForBlockedCardState())
                {
                    _response.IsSuccessful = false;
                    _response.Status = HttpStatusCode.Unauthorized;
                    _response.Detail = new List<String>() { "The card is blocked" };
                    return Unauthorized(_response);
                }
                    //Verify card pin
                if ( bankCardFiltered.pin != request.pin)
                {
                    _response.IsSuccessful = false;
                    bankCardFiltered.failed_attempts += 1;
                    if (bankCardFiltered.failed_attempts >= GetMaxFailedAttempts())
                    {
                        bankCardFiltered.id_card_state = await GetIdForBlockedCardState();
                        _response.Detail = new List<String>() { "PIN invalid", "The card has been blocked" };
                    }
                    else
                    {
                        int remaining_attempts = GetMaxFailedAttempts() - bankCardFiltered.failed_attempts;
                        _response.Detail = new List<String>() { "PIN invalid", "remaining_attempts: " + (remaining_attempts >= 0 ? remaining_attempts : 0) };
                    }
                    _response.Status = HttpStatusCode.Unauthorized;

                    BankCard updatedErrorBankCard = _mapper.Map<BankCard>(bankCardFiltered);
                    await _repo.Update(updatedErrorBankCard);
                    return Unauthorized(_response);
                }

                /*Reset Bank Card Failed Attempts*/
                bankCardFiltered.failed_attempts = 0;
                BankCard updatedBankCard = _mapper.Map<BankCard>(bankCardFiltered);
                await _repo.Update(updatedBankCard);

                _response.Status = HttpStatusCode.OK;
                _response.Result = bankCardFiltered;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _logger.LogError("CheckBankCardPin", ex.Message);
                _response.IsSuccessful = false;
                _response.Status = HttpStatusCode.InternalServerError;
                _response.Detail = new List<string>() { ex.ToString() };
                return _response;
            }
        }


    }
}
