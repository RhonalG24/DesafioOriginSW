using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.Models.Requests.BankCard
{
    public class CheckBankCardNumberRequest
    {
        [Required]
        [MaxLength(16), MinLength(16)]
        public String bank_card_number {  get; set; }
    }
}
