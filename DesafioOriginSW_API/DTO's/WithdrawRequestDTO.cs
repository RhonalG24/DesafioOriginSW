using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.DTO_s
{
    public class WithdrawRequestDTO
    {
        [Required]
        public Int32 bank_card_id { get; set; }
        [Required]
        public Double withdrawal_amount { get; set; }
    }
}
