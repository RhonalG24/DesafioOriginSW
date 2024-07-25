namespace DesafioOriginSW_API.Models.Responses.BankCard
{
    public class CheckBankCardPinResponse
    {
        public int id_bank_card { get; set; }

        public int id_account { get; set; }

        public string number { get; set; }

        public int id_card_state { get; set; }

        public DateOnly expiry_date { get; set; }

        public int failed_attempts { get; set; }
    }
}
