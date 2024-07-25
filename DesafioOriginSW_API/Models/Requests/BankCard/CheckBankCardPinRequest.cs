using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.Models.Requests.BankCard
{
    public class CheckBankCardPinRequest
    {
        [Required]
        public Int32 id_bank_card { get; set; }

        [Required]
        [MaxLength(4), MinLength(4)]
        public String pin { get; set; }
    }
}
