using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.Models.Requests.Operation
{
    public class WithdrawalRequest
    {
        [Required]
        public int bank_card_id { get; set; }
        [Required]
        public double withdrawal_amount { get; set; }
    }
}
