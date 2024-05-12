using AutoMapper;
using DesafioOriginSW_API.Data;
using DesafioOriginSW_API.DTO_s;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DesafioOriginSW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankCardController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IBankCardRepository _repo;
        private readonly ICardStateRepository _repoCardState;
        private readonly IAccountRepository _repoAccount;
        private readonly IOperationTypeRepository _repoOperationType;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        protected APIResponse _response;


        public BankCardController(ILogger<OperationController> logger,
                                    IBankCardRepository repo,
                                    ICardStateRepository repoCardState,
                                    IAccountRepository repoAccount,
                                    IOperationTypeRepository repoOperationType,
                                    AppDbContext db,
                                    IMapper mapper)
        {
            _logger = logger;
            _repo = repo;
            _repoCardState = repoCardState;
            _repoAccount = repoAccount;
            _repoOperationType = repoOperationType;
            _db = db;
            _mapper = mapper;
            _response = new();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAllBankCards()
        {
            try
            {
                _logger.LogInformation("Get all bank cards");
                IEnumerable<BankCard> bankCardList = await _repo.GetAll();
                _response.Result = bankCardList;
                _response.IsSuccessful = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get all BankCards", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }

        }

        [HttpGet("id:int", Name = "GetBankCard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetBankCard(int id)
        {
            try
            {
                var bankCardFiltered = await _repo.Get(v => v.id_bank_card == id);
                if (bankCardFiltered == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);

                }
                _response.Result = bankCardFiltered;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get Bank Card", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }

        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateBankCard([FromBody] CreateBankCardDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                //verify that associated account exist
                if (await _repoAccount.Get(v => v.id_account == createDTO.id_account) == null)
                {
                    _logger.LogError("Account id doesn't exist");
                    ModelState.AddModelError("ErrorsMessage", "Account id not found");
                    return BadRequest(ModelState);
                }

                BankCard newBankCard = _mapper.Map<BankCard>(createDTO);
                newBankCard.expiry_date = DateOnly.FromDateTime(DateTime.Now.AddYears(10));
                newBankCard.id_card_state = 1; //Active

                await _repo.Create(newBankCard);
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = newBankCard;
                return CreatedAtRoute("GetBankCard", new { id = newBankCard.id_bank_card }, _response);

            }
            catch (Exception ex)
            {
                _logger.LogError("Create Bank Card", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpGet("Check/Number/bank_card_number:string", Name = "CheckBankCardNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CheckBankCardNumberDTO(String bank_card_number)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                
                BankCard bankCardFiltered = await _repo.Get(v => v.number == bank_card_number);
                if (bankCardFiltered == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Result = bank_card_number;
                    return NotFound(_response);

                }
                //Verify if it's blocked
                if (bankCardFiltered.id_card_state == await GetIdForBlockedCardState())
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.Unauthorized;
                    _response.Result = bank_card_number;
                    _response.ErrorsMessage = new List<String>() { "The card is blocked" };
                    return Unauthorized(_response);
                }

                CheckBankCardNumberRespondDTO BankCardRespond = _mapper.Map<CheckBankCardNumberRespondDTO>(bankCardFiltered);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = BankCardRespond;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _logger.LogError("CheckBankCardNumber", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }
        }
        
        [HttpPost("Check/Pin/bank_card_id:int", Name = "CheckCardPin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CheckBankCardPin(int bank_card_id, [FromBody] CheckBankCardPinDTO checkCardPinDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                
                BankCard bankCardFiltered = await _repo.Get(v => v.id_bank_card == checkCardPinDTO.id_bank_card);
                if (bankCardFiltered == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Result = checkCardPinDTO;
                    return NotFound(_response);

                }
                //Verify if it's blocked
                if (bankCardFiltered.id_card_state == await GetIdForBlockedCardState())
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.Unauthorized;
                    _response.Result = checkCardPinDTO;
                    _response.ErrorsMessage = new List<String>() { "The card is blocked" };
                    return Unauthorized(_response);
                }
                    //Verify card pin
                if ( bankCardFiltered.pin != checkCardPinDTO.pin)
                {
                    _response.IsSuccessful = false;
                    _response.Result = checkCardPinDTO;
                    bankCardFiltered.failed_attempts += 1;
                    if( bankCardFiltered.failed_attempts >= 4)
                    {
                        bankCardFiltered.id_card_state = await GetIdForBlockedCardState();
                        _response.ErrorsMessage = new List<String>() { "PIN invalid", "The card has been blocked" };
                    }
                    else
                    {
                        int remaining_attempts = 4 - bankCardFiltered.failed_attempts;
                        _response.ErrorsMessage = new List<String>() { "PIN invalid", "remaining_attempts: " + (remaining_attempts >= 0 ? remaining_attempts : 0) };
                    }
                    _response.StatusCode = HttpStatusCode.Unauthorized;

                    BankCard updatedErrorBankCard = _mapper.Map<BankCard>(bankCardFiltered);
                    await _repo.Update(updatedErrorBankCard);
                    return Unauthorized(_response);
                }

                /*Reset Bank Card Failed Attempts*/
                bankCardFiltered.failed_attempts = 0;
                BankCard updatedBankCard = _mapper.Map<BankCard>(bankCardFiltered);
                await _repo.Update(updatedBankCard);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = bankCardFiltered;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _logger.LogError("CheckBankCardPin", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        private async Task<int> GetIdForBlockedCardState()
        {
            CardState cardState = await _repoCardState.Get(v => v.name == "bloqueada");

            return cardState.id_card_state;
        }
    }
}
