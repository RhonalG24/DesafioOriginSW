using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Requests.Account;
using DesafioOriginSW_API.Models.Responses.Account;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;

namespace DesafioOriginSW_API.Handlers.IHandler
{
    public interface IAccountHandler
    {
        Task<Result<APIResponse<GetAllAccountsResponse>>> GetAllAccounts();

        Task<Result<APIResponse<GetAccountResponse>>> GetAccount(int id);

        Task<Result<APIResponse<UpdateAccountResponse>>> UpdateAccount(UpdateAccountRequest request);

        Task<Result<APIResponse<UpdatePartialAccountResponse>>> UpdatePartialAccount(JsonPatchDocument<UpdatePartialAccountRequest> request);
    }
}
