using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Entities;
using DesafioOriginSW_API.Models.Requests.Account;
using DesafioOriginSW_API.Models.Responses.Account;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;

namespace DesafioOriginSW_API.Handlers.IHandler
{
    public interface IAccountHandler
    {
        Task<Result<GetAllAccountsResponse>> GetAllAccounts();

        Task<Result<GetAccountResponse>> GetAccount(int id);

        Task<Result<UpdateAccountResponse>> UpdateAccount(UpdateAccountRequest request);

        Task<Account> GetAccountById(int id);

        //Task<Result<APIResponse<UpdatePartialAccountResponse>>> UpdatePartialAccount(JsonPatchDocument<UpdatePartialAccountRequest> request);
    }
}
