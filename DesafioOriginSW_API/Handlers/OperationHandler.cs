
using DesafioOriginSW_API.DTO_s;
using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Repository.IRepository;
using Operation = DesafioOriginSW_API.Models.Operation;
using DesafioOriginSW_API.Models.Request;

namespace DesafioOriginSW_API.Handlers
{
    public class OperationHandler : IOperationHandler
    {
        private readonly IOperationRepository _operationRepository;
        private readonly IOperationTypeRepository _operationTypeRepository;
        private readonly IBankCardRepository _bankCardRepository;
        private readonly IAccountRepository _accountRepository;

        public OperationHandler(
            IOperationRepository operationRepository,
            IOperationTypeRepository operationTypeRepository,
            IBankCardRepository bankCardRepository,
            IAccountRepository accountRepository
        )
        {
            _operationRepository = operationRepository ?? throw new ArgumentNullException(nameof(operationRepository));
            _operationTypeRepository = operationTypeRepository ?? throw new ArgumentNullException(nameof(operationTypeRepository));
            _bankCardRepository = bankCardRepository ?? throw new ArgumentNullException(nameof(bankCardRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<IEnumerable<Operation>> GetAllOperations()
        {
            List<Operation> result = await _operationRepository.GetAll();

            return await Task.FromResult(result);
        }

        public async Task<Operation> GetOperationById(int id)
        {
            var operationFiltered = await _operationRepository.Get(v => v.id_operation == id);

            return await Task.FromResult(operationFiltered);
        }

        public async Task<GetBalanceDTO> GetBalance(int bank_card_id)
        {
            var bankCardFiltered = await _bankCardRepository.Get(v => v.id_bank_card == bank_card_id);

            if (bankCardFiltered == null)
                throw new Exception("BankCard not exist");

            var accountRelated = await _accountRepository.Get(v => v.id_account == bankCardFiltered.id_account);
            if (accountRelated == null)
                throw new Exception("Account not exist");

            GetBalanceDTO checkBalanceDTO = new()
            {
                balance = accountRelated.balance,
                bank_card_number = bankCardFiltered.number,
                bank_card_expiry_date = bankCardFiltered.expiry_date
            };

            await CreateOperation(bankCardFiltered.id_bank_card, await GetIdForBalanceOperation(), accountRelated.balance);

            return await Task.FromResult(checkBalanceDTO);
        }

        public async Task<WithdrawBalanceDTO> WithdrawalBalance(WithdrawalRequest request)
        {

            var bankCardFiltered = await _bankCardRepository.Get(v => v.id_bank_card == request.bank_card_id);

            if (bankCardFiltered == null)
                throw new Exception("BankCard not exist");

            var accountRelated = await _accountRepository.Get(v => v.id_account == bankCardFiltered.id_account);

            if(accountRelated == null)
                throw new Exception("Account not exist");

            if (!IsValidAmount(request.withdrawal_amount, accountRelated.balance))
                throw new Exception("Insufficient funds");

            Double newBalance = PerformWithdrawal(request.withdrawal_amount, accountRelated.balance);

            WithdrawBalanceDTO withdrawBalanceDTO = new()
            {
                balance = newBalance,
                bank_card_number = bankCardFiltered.number,
                withdrawal_amount = request.withdrawal_amount,
                datetime = DateTime.Now
            };

            accountRelated.balance = newBalance;
            await _accountRepository.Update(accountRelated);
            await CreateOperation(bankCardFiltered.id_bank_card, await GetIdForWithdrawOperation(), request.withdrawal_amount);

            return withdrawBalanceDTO;
        }


        private async Task CreateOperation(int id_bank_card, int operationType, double? amount)
        {
            Operation newOperation = new()
            {
                id_bank_card = id_bank_card,
                id_operation_type = operationType,
                amount = amount,
                date = DateTime.Now
            };

            await _operationRepository.Create(newOperation);
        }

        private async Task<int> GetIdForBalanceOperation()
        {
            OperationType operationType = await _operationTypeRepository.Get(v => v.name == "balance");

            return operationType.id_operation_type;
        }

        private async Task<int> GetIdForWithdrawOperation()
        {
            OperationType operationType = await _operationTypeRepository.Get(v => v.name == "retiro");

            return operationType.id_operation_type;
        }

        private bool IsValidAmount(Double amount, Double balance)
        {
            return amount <= balance;
        }

        private Double PerformWithdrawal(Double amount, Double balance)
        {
            Double newAmount = balance - amount;
            return newAmount;
        }


    }
}
