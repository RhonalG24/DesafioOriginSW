namespace DesafioOriginSW_API.Models.Responses.BankCard
{
    public class GetAllBankCardResponse
    {
        public IEnumerable<GetBankCardResponse> bankCards { get; set; }
    }
}
