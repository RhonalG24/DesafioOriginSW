using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.Models.Responses.BankCard
{
    public class CheckBankCardNumberResponse
    {
        public Int32 id_bank_card { get; set; }

        public String number { get; set; }
    }
}
