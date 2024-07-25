using AutoMapper;
using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Entities;
using DesafioOriginSW_API.Models.Errors;
using DesafioOriginSW_API.Models.Requests.Account;
using DesafioOriginSW_API.Models.Responses.Account;
using DesafioOriginSW_API.Repository.IRepository;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DesafioOriginSW_API.Handlers
{
    public class AccountHandler : IAccountHandler
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;


        public AccountHandler(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<Result<APIResponse<GetAccountResponse>>> GetAccount(int id)
        {
            var account = _mapper.Map<GetAccountResponse>(await GetAccountById(id));
            APIResponse<GetAccountResponse> response = new();
            return response;
        }

        public async Task<Result<APIResponse<GetAllAccountsResponse>>> GetAllAccounts()
        {
            GetAllAccountsResponse accountList = new();

            accountList.Accounts = await _accountRepository.GetAll();
            //if (accountList.Accounts != null || accountList.Accounts?.Count() == 0)
            //    return new NotFoundError("No hay registros cargados.");

            APIResponse<GetAllAccountsResponse> response = new(
                    result: accountList,
                    isSuccessful: true,
                    statusCode: HttpStatusCode.OK,
                    errorsMessage: null
                );

            return Result.Ok(response);
        }

        public async Task<Result<APIResponse<UpdateAccountResponse>>> UpdateAccount(UpdateAccountRequest request)
        {

            if (request == null) 
                return new BadRequestError("El request no puede estar vacío.");

            var oldAccount = await GetAccountById(request.id_account);
            if (oldAccount == null)  
                return new NotFoundError("El id solicitado no existe.");
            

            //Account newAccount = _mapper.Map<Account>(request);
            Account newAccount = new();
            newAccount.id_account = request?.id_account ?? oldAccount.id_account;
            newAccount.balance = request?.balance ?? oldAccount.balance;

            await _accountRepository.Update(newAccount);

            APIResponse<UpdateAccountResponse> _response = new( 
                result: _mapper.Map<UpdateAccountResponse>(newAccount) ?? throw new ArgumentNullException(nameof(response)),
                isSuccessful: true,
                statusCode: HttpStatusCode.OK,
                errorsMessage: null
                );

            return Result.Ok(_response);

        }

        /*
        public async Task<Result<APIResponse<UpdatePartialAccountResponse>>> UpdatePartialAccount(JsonPatchDocument<UpdatePartialAccountRequest> request)
        {
            if (request == null) 
                return new BadRequestError("El request no puede estar vacío.");

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
        }
        */

        public async Task<Account> GetAccountById(int id)
        {
            return await _accountRepository.Get(x => x.id_account == id);
        }

        private BadRequestObjectResult ReturnModelStateErrors(ModelStateDictionary modelState)
        {
            var errors = modelState.Values
                                   .SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();

            var result = Result.Fail("Validation failed")
                               .WithErrors(errors);

            return BadRequest(result);
        }
    }
}
