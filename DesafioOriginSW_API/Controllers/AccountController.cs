using AutoMapper;
using DesafioOriginSW_API.DTO_s;
using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DesafioOriginSW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAccountRepository _repo;
        private readonly IMapper _mapper;
        private readonly IAccountHandler _handler;
        protected APIResponse _response;

        public AccountController(IAccountHandler handler, ILogger<AccountController> logger, IAccountRepository repo, IMapper mapper)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllAccounts()
        {
            try
            {
                _logger.LogInformation("Get all accounts");
                IEnumerable<Account> accountList = await _handler.GetAllAccounts();
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = accountList ?? throw new ArgumentNullException(nameof(accountList));

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Update account", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpGet("{id}", Name = "GetAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAccount(int id)
        {
            try
            {
                //_logger.LogInformation("Get all accounts");
                var accountFiltered = await _repo.Get(v => v.id_account == id);
                if (accountFiltered == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);

                }
                _response.Result = accountFiltered ?? throw new ArgumentNullException(nameof(accountFiltered));
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get account", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }

        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateAccount(int id, [FromBody] UpdateAccountDTO accountDTO)
        {
            try
            {
                if (accountDTO == null) return BadRequest();
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var oldAccount = _repo.Get(v => v.id_account == accountDTO.id_account, tracked: false);
                if (oldAccount == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful = false;
                    return NotFound(_response);
                }

                Account newAccount = _mapper.Map<Account>(accountDTO);
                newAccount.id_account = id;

                await _repo.Update(newAccount);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = newAccount ?? throw new ArgumentNullException(nameof(newAccount));

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Update account", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }

        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdatePartialAccount(int id, JsonPatchDocument<UpdateAccountDTO> accountDTO)
        {
            if (accountDTO == null) return BadRequest();

            var oldAccount = await _repo.Get(v => v.id_account == id, tracked: false);

            if (oldAccount == null) return NotFound();

            UpdateAccountDTO newAccount = _mapper.Map<UpdateAccountDTO>(oldAccount);

            accountDTO.ApplyTo(newAccount, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Account accountUpdated = _mapper.Map<Account>(newAccount);
            accountUpdated.id_account = id;

            await _repo.Update(accountUpdated);
            return Ok(accountUpdated.id_account);
        }
    }
}
