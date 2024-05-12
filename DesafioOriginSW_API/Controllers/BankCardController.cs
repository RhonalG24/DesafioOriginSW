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
        private readonly IAccountRepository _repoAccount;
        private readonly IOperationTypeRepository _repoOperationType;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        protected APIResponse _response;


        public BankCardController(ILogger<OperationController> logger,
                                    IBankCardRepository repo,
                                    IAccountRepository repoAccount,
                                    IOperationTypeRepository repoOperationType,
                                    AppDbContext db,
                                    IMapper mapper)
        {
            _logger = logger;
            _repo = repo;
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

        
    //        var order = context.Orders
    //.Include(o => o.Customer) // Incluye los datos relacionados de Customer
    //.FirstOrDefault();


        //}

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

            //TODO: demás operaciones, como: bloquear tarjeta, retirar dinero, verificar dinero en la cuenta, consultar saldo
        }
    }
}
