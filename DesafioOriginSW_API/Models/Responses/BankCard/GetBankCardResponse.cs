using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.Models.Responses.BankCard
{
    public class GetBankCardResponse
    {
        public int id_bank_card { get; set; }
        
        public int id_account { get; set; }

        public string number { get; set; }

        public int id_card_state { get; set; }

        public DateOnly expiry_date { get; set; }

        public int failed_attempts { get; set; }

    }
}
