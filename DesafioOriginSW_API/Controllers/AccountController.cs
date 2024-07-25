using AutoMapper;
using DesafioOriginSW_API.DTO_s;
using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Entities;
using DesafioOriginSW_API.Models.Errors;
using DesafioOriginSW_API.Models.Requests.Account;
using DesafioOriginSW_API.Models.Responses.Account;
using DesafioOriginSW_API.Repository.IRepository;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace DesafioOriginSW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ResultsControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAccountRepository _repo;
        private readonly IMapper _mapper;
        private readonly IAccountHandler _handler;
        //protected APIResponse<> _response;

        public AccountController(IAccountHandler handler, ILogger<AccountController> logger, IAccountRepository repo, IMapper mapper)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            //_response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse<GetAllAccountsResponse>>> GetAllAccounts()
        {
            APIResponse<GetAllAccountsResponse> _response = new();
            try
            {
                _logger.LogInformation("Get all accounts");
                GetAllAccountsResponse accountList = await _handler.GetAllAccounts();
                _response.Status = HttpStatusCode.OK;
                _response.Result = accountList ?? throw new ArgumentNullException(nameof(accountList));

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Update account", ex.Message);
                _response.IsSuccessful = false;
                _response.Status = HttpStatusCode.InternalServerError;
                _response.Detail = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        #region [HttpGet("{id}", Name = "GetAccount")]
        [HttpGet("{id}", Name = "GetAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<APIResponse<Account>>> GetAccount(int id)
        {
            APIResponse<Account> _response = new();
            try
            {
                //_logger.LogInformation("Get all accounts");
                var accountFiltered = await _repo.Get(v => v.id_account == id);
                if (accountFiltered == null)
                {
                    _response.IsSuccessful = false;
                    _response.Status = HttpStatusCode.NotFound;
                    return NotFound(_response);

                }
                else
                {
                    _response.Result = accountFiltered ?? throw new ArgumentNullException(nameof(accountFiltered));
                    _response.Status = HttpStatusCode.OK;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Get account", ex.Message);
                _response.IsSuccessful = false;
                _response.Status = HttpStatusCode.InternalServerError;
                _response.Detail = new List<string>() { ex.ToString() };
                return _response;
            }

        }
        
        /*#region [HttpGet("{id}", Name = "GetAccount")]
        [HttpGet("{id}", Name = "GetAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<APIResponse<Account>>> GetAccount(int id)
        {
            APIResponse<Account> _response = new();
            try
            {
                //_logger.LogInformation("Get all accounts");
                var accountFiltered = await _repo.Get(v => v.id_account == id);
                if (accountFiltered == null)
                {
                    _response.IsSuccessful = false;
                    _response.Status = HttpStatusCode.NotFound;
                    return NotFound(_response);

                }
                else
                {
                    _response.Result = accountFiltered ?? throw new ArgumentNullException(nameof(accountFiltered));
                    _response.Status = HttpStatusCode.OK;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Get account", ex.Message);
                _response.IsSuccessful = false;
                _response.Status = HttpStatusCode.InternalServerError;
                _response.Detail = new List<string>() { ex.ToString() };
                return _response;
            }

        }*/

        #region [HttpPut("{id}")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<APIResponse<Account>>> UpdateAccount(int id, [FromBody] UpdateAccountRequest request)
        {
            if (!IsModelStateValid(ModelState))
            {
                return ReturnModelStateErrors(ModelState);
            }

            var response = await _handler.UpdateAccount(request); 

            return Ok(response);


        }        

        //private BadRequestObjectResult ReturnModelStateErrors(ModelStateDictionary modelState)
        //{
        //    var errors = ModelState.Values
        //                           .SelectMany(v => v.Errors)
        //                           .Select(e => e.ErrorMessage)
        //                           .ToList();

        //    var result = Result.Fail("Validation failed")
        //                       .WithErrors(errors);

        //    return BadRequest(result);
        //}
        
        /*#region [HttpPut("{id}")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<APIResponse<Account>>> UpdateAccount(int id, [FromBody] UpdateAccountRequest request)
        {
            APIResponse<Account> _response = new();
            try
            {
                if (request == null) return new BadRequestError();
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var oldAccount = _repo.Get(v => v.id_account == request.id_account, tracked: false);
                if (oldAccount == null)
                {
                    _response.Status = HttpStatusCode.NotFound;
                    _response.IsSuccessful = false;
                    return NotFound(_response);
                }

                Account newAccount = _mapper.Map<Account>(request);
                newAccount.id_account = id;

                await _repo.Update(newAccount);
                _response.Status = HttpStatusCode.OK;
                _response.Result = newAccount ?? throw new ArgumentNullException(nameof(newAccount));

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Update account", ex.Message);
                _response.IsSuccessful = false;
                _response.Status = HttpStatusCode.InternalServerError;
                _response.Detail = new List<string>() { ex.ToString() };
                return _response;
            }

        }*/

        /*#region [HttpPatch("{id}")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<string>> UpdatePartialAccount(int id, JsonPatchDocument<UpdatePartialAccountRequest> request)
        {
            if (request == null) return BadRequest();

            var oldAccount = await _repo.Get(v => v.id_account == id, tracked: false);

            if (oldAccount == null) return NotFound();

            UpdateAccountDTO newAccount = _mapper.Map<UpdateAccountDTO>(oldAccount);

            request.ApplyTo(newAccount, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Account accountUpdated = _mapper.Map<Account>(newAccount);
            accountUpdated.id_account = id;

            var response = _handler.UpdatePartialAccount(request);
            return Ok();
            await _repo.Update(accountUpdated);
            return Ok(accountUpdated.id_account);
        }*/
    }
}
