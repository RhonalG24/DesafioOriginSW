using AutoMapper;
using DesafioOriginSW_API.Data;
using DesafioOriginSW_API.DTO_s;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace DesafioOriginSW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IOperationRepository _repo;
        private readonly IBankCardRepository _repoBankCard;
        private readonly IAccountRepository _repoAccount;
        private readonly IOperationTypeRepository _repoOperationType;
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;
        protected APIResponse _response;


        public OperationController(ILogger<OperationController> logger,
                                    IOperationRepository repo,
                                    IBankCardRepository repoBankCard,
                                    IAccountRepository repoAccount,
                                    IOperationTypeRepository repoOperationType,
                                    IMapper mapper)
        {
            _logger = logger;
            _repo = repo;
            _repoBankCard = repoBankCard;
            _repoAccount = repoAccount;
            _repoOperationType = repoOperationType;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAllOperations()
        {
            try
            {
                _logger.LogInformation("Get all operations");
                IEnumerable<Operation> operationList = await _repo.GetAll();
                _response.Result = operationList;
                _response.IsSuccessful = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get all operations", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }

        }

        [HttpGet("id:int", Name = "GetOperation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetOperation(int id)
        {
            try
            {
                //_logger.LogInformation("Get all accounts");
                var operationFiltered = await _repo.Get(v => v.id_operation == id);
                if (operationFiltered == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);

                }
                _response.Result = operationFiltered;
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateOperation([FromBody] CreateOperationDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                //verify that associated card exist
                if (await _repoBankCard.Get(v => v.id_bank_card == createDTO.id_bank_card) == null)
                {
                    _logger.LogError("Bank card id not exist");
                    ModelState.AddModelError("Bank card id not exist", "Bank card id not found");
                    return BadRequest(ModelState);

                }
                //verify that operation type exist
                if (await _repoOperationType.Get(v => v.id_operation_type == createDTO.id_operation_type) == null)
                {
                    _logger.LogError("Operation type id not exist");
                    ModelState.AddModelError("Operation type id not exist", "Operation type id not found");
                    return BadRequest(ModelState);

                }

                Operation newOperation = new();
                newOperation.date = DateTime.Now;
                await _repo.Create(newOperation);
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = newOperation;
                return CreatedAtRoute("GetOperation", new { id = newOperation.id_operation }, _response);

            }
            catch (Exception ex)
            {
                _logger.LogError("Create operation", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        private async Task<int> GetIdForBalanceOperation()
        {
            OperationType operationType = await _repoOperationType.Get(v => v.name == "balance");

            return operationType.id_operation_type;
        }

        private async Task<int> GetIdForWithdrawOperation()
        {
            OperationType operationType = await _repoOperationType.Get(v => v.name == "retiro");

            return operationType.id_operation_type;
        }

        private bool IsValidAmount(Double amount, Double balance)
        {
            return amount <= balance;
        }

        private Double PerformWithdrawal(Double amount, Double balance)
        {
            return balance - amount;
        }

        [HttpPost("Balance/bank_card_id:int", Name = "CheckBalance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CheckBalance(int bank_card_id)
        {
            try
            {
                var bankCardFiltered = await _repoBankCard.Get(v => v.id_bank_card == bank_card_id);
                if (bankCardFiltered == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                if (await _repoAccount.Get(v => v.id_account == bankCardFiltered.id_account) == null)
                {
                    _logger.LogError("Account id doesn't exist");
                    ModelState.AddModelError("ErrorsMessage", "Account id not found");
                    return BadRequest(ModelState);
                }
                var accountRelated = await _repoAccount.Get(v => v.id_account == bankCardFiltered.id_account);

                CheckBalanceDTO checkBalanceDTO = new CheckBalanceDTO();
                checkBalanceDTO.balance = accountRelated.balance;
                checkBalanceDTO.bank_card_number = bankCardFiltered.number;
                checkBalanceDTO.bank_card_expiry_date = bankCardFiltered.expiry_date;

                //var operationType = await _repoOperationType.Get(v => v.name == "balance");
                Operation newOperation = new Operation();
                newOperation.id_bank_card = bankCardFiltered.id_bank_card;
                newOperation.id_operation_type = await GetIdForBalanceOperation();
                newOperation.amount = accountRelated.balance;
                newOperation.date = DateTime.Now;

                await _repo.Create(newOperation);

                _response.Result = checkBalanceDTO;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetOperation", new { id = newOperation.id_operation }, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError("CheckBalance", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpPost("Withdraw/bank_card_id:int", Name = "WithdrawBalance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> WithdrawBalance(int bank_card_id, [FromBody] Double withdrawalAmount)
        {
            try
            {
                var bankCardFiltered = await _repoBankCard.Get(v => v.id_bank_card == bank_card_id);
                if (bankCardFiltered == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                if (await _repoAccount.Get(v => v.id_account == bankCardFiltered.id_account) == null)
                {
                    _logger.LogError("Account id doesn't exist");
                    ModelState.AddModelError("ErrorsMessage", "Account id not found");
                    return BadRequest(ModelState);
                }
                var accountRelated = await _repoAccount.Get(v => v.id_account == bankCardFiltered.id_account);

                if(!IsValidAmount(withdrawalAmount, accountRelated.balance))
                {
                    _logger.LogError("Insufficient funds");
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorsMessage = new List<String>() { "Insufficient funds" };
                    return BadRequest(_response);
                }

                Double newBalance = PerformWithdrawal(withdrawalAmount, accountRelated.balance);

                WithdrawBalanceDTO withdrawBalanceDTO = new WithdrawBalanceDTO();
                withdrawBalanceDTO.balance = newBalance;
                withdrawBalanceDTO.bank_card_number = bankCardFiltered.number;
                withdrawBalanceDTO.withdrawal_amount = withdrawalAmount;
                withdrawBalanceDTO.datetime = DateTime.Now;

                //var operationType = await _repoOperationType.Get(v => v.name == "balance");
                Operation newOperation = new Operation();
                newOperation.id_bank_card = bankCardFiltered.id_bank_card;
                newOperation.id_operation_type = await GetIdForWithdrawOperation();
                newOperation.amount = withdrawalAmount;
                newOperation.date = DateTime.Now;

                await _repo.Create(newOperation);

                _response.Result = withdrawBalanceDTO;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetOperation", new { id = newOperation.id_operation }, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError("WithdrawBalance", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }
        }



    }
}
