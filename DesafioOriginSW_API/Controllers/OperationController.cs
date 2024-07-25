using AutoMapper;
using DesafioOriginSW_API.Data;
using DesafioOriginSW_API.DTO_s;
using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Entities;
using DesafioOriginSW_API.Models.Request;
using DesafioOriginSW_API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace DesafioOriginSW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : ResultsControllerBase
    {
        private readonly ILogger _logger;
        private readonly IOperationHandler _handler;
        private readonly IOperationRepository _repo;
        private readonly IBankCardRepository _repoBankCard;
        private readonly IAccountRepository _repoAccount;
        private readonly IOperationTypeRepository _repoOperationType;
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;


        public OperationController(
            ILogger<OperationController> logger,
            IOperationHandler handler,
            IOperationRepository repo,
            IBankCardRepository repoBankCard,
            IAccountRepository repoAccount,
            IOperationTypeRepository repoOperationType,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _repoBankCard = repoBankCard ?? throw new ArgumentNullException(nameof(repoBankCard));
            _repoAccount = repoAccount ?? throw new ArgumentNullException(nameof(repoAccount));
            _repoOperationType = repoOperationType ?? throw new ArgumentNullException(nameof(repoOperationType));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region [HttpGet]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<APIResponse<IEnumerable<Operation>>>> GetAllOperations()
        {
            APIResponse<IEnumerable<Operation>> _response = new();
            try
            {
                _logger.LogInformation("Get all operations");
                IEnumerable<Operation> operationList = await _handler.GetAllOperations();
                _response.Result = operationList;
                _response.IsSuccessful = true;
                _response.Status = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get all operations", ex.Message);
                _response.IsSuccessful = false;
                _response.Status = HttpStatusCode.InternalServerError;
                _response.Detail = new List<string>() { ex.ToString() };
                return _response;
            }

        }

        #region [HttpGet("{id}", Name = "GetOperation")]
        [HttpGet("{id}", Name = "GetOperation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<APIResponse<Operation>>> GetOperationById(int id)
        {
            APIResponse<Operation> _response = new();
            try
            {
                //_logger.LogInformation("Get all accounts");
                var operationFiltered = await _handler.GetOperationById(id);

                if (operationFiltered == null)
                {
                    _response.IsSuccessful = false;
                    _response.Status = HttpStatusCode.NotFound;
                    return NotFound(_response);

                }
                _response.Result = operationFiltered;
                _response.Status = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get operation", ex.Message);
                _response.IsSuccessful = false;
                _response.Status = HttpStatusCode.InternalServerError;
                _response.Detail = new List<string>() { ex.ToString() };
                return _response;
            }

        }

        #region [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        #endregion
        public async Task<ActionResult<APIResponse<Operation>>> CreateOperation([FromBody] CreateOperationDTO createDTO)
        {
            APIResponse<Operation> _response = new();
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
                _response.Status = HttpStatusCode.Created;
                _response.Result = newOperation;
                return CreatedAtRoute("GetOperation", new { id = newOperation.id_operation }, _response);

            }
            catch (Exception ex)
            {
                _logger.LogError("Create operation", ex.Message);
                _response.IsSuccessful = false;
                _response.Status = HttpStatusCode.InternalServerError;
                _response.Detail = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        #region [HttpGet("balance/{bank_card_id}", Name = "GetBalance")]
        [HttpGet("balance/{bank_card_id}", Name = "GetBalance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public async Task<ActionResult<APIResponse<GetBalanceDTO>>> GetBalance(int bank_card_id)
        {
            APIResponse<GetBalanceDTO> _response = new();
            try
            {
                GetBalanceDTO balance = await _handler.GetBalance(bank_card_id);

                _response.Result = balance;
                _response.Status = HttpStatusCode.Created;

                return await Task.FromResult(Ok(_response));
            }
            catch (Exception ex)
            {
                _logger.LogError("CheckBalance", ex.Message);
                _response.IsSuccessful = false;
                _response.Status = HttpStatusCode.InternalServerError;
                _response.Detail = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        #region [HttpPost("withdrawal", Name = "WithdrawalBalance")]
        [HttpPost("withdrawal", Name = "WithdrawalBalance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public async Task<ActionResult<APIResponse<WithdrawBalanceDTO>>> WithdrawalBalance([FromBody] WithdrawalRequest request)
        {
            APIResponse<WithdrawBalanceDTO> _response = new();
            try
            {
                var withdrawBalance = await _handler.WithdrawalBalance(request);

                _response.Result = withdrawBalance;
                _response.Status = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("WithdrawBalance", ex.Message);
                _response.IsSuccessful = false;
                _response.Status = HttpStatusCode.InternalServerError;
                _response.Detail = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
        }
    }
}
