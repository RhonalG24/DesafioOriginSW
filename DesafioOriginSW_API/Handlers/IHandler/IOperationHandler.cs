using DesafioOriginSW_API.DTO_s;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Request;

namespace DesafioOriginSW_API.Handlers.IHandler
{
    public interface IOperationHandler
    {
        Task<IEnumerable<Operation>> GetAllOperations();
        Task<Operation> GetOperationById(int id);
        Task<GetBalanceDTO> GetBalance(int bankCardId);
        Task<WithdrawBalanceDTO> WithdrawalBalance(WithdrawalRequest withdrawalRequest);
    }
}
