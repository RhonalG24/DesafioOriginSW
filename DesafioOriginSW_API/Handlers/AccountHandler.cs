using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Repository.IRepository;

namespace DesafioOriginSW_API.Handlers
{
    public class AccountHandler : IAccountHandler
    {
        public IAccountRepository _accountRepository;

        public AccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            IEnumerable<Account> accountList = await _accountRepository.GetAll();

            return await Task.FromResult(accountList);
        }
    }
}
