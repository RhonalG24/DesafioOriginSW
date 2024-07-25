using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.Models.Request
{
    public class WithdrawalRequest
    {
        [Required]
        public Int32 bank_card_id { get; set; }
        [Required]
        public Double withdrawal_amount { get; set; }
    }
}
