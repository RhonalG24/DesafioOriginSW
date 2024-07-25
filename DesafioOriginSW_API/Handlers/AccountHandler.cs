using AutoMapper;
using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Handlers.Shared;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Entities;
using DesafioOriginSW_API.Models.Errors;
using DesafioOriginSW_API.Models.Requests.Account;
using DesafioOriginSW_API.Models.Responses.Account;
using DesafioOriginSW_API.Repository.IRepository;
using FluentResults;
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
        
        public async Task<Result<GetAccountResponse>> GetAccount(int id)
        {
            if (id < 0)
                return new RequestValidationError("Id no válido");

            Account account = await GetAccountById(id);
            if (account == null)
                return new NotFoundError("El id solicitado no existe.");

            var accountMapped = _mapper.Map<GetAccountResponse>(account);
 
            return Result.Ok(accountMapped);
        }

        public async Task<Result<GetAllAccountsResponse>> GetAllAccounts()
        {
            GetAllAccountsResponse accountList = new();

            accountList.Accounts = await _accountRepository.GetAll();

            if (accountList.Accounts == null)
                return new NotFoundError("no se encontraron registros");

            

            return Result.Ok(accountList);
        }

        public async Task<Result<UpdateAccountResponse>> UpdateAccount(UpdateAccountRequest request)
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

            var response = _mapper.Map<UpdateAccountResponse>(newAccount);

            return Result.Ok(response);

        }

        /*public async Task<Result<APIResponse<UpdatePartialAccountResponse>>> UpdatePartialAccount(JsonPatchDocument<UpdatePartialAccountRequest> request)
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

    }
}
