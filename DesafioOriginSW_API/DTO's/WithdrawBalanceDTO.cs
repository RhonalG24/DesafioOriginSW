using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.DTO_s
{
    public class WithdrawBalanceDTO
    {
        public Double balance { get; set; }

        [MaxLength(16), MinLength(16)]
        public String bank_card_number { get; set; }
        public DateTime datetime { get; set; }

        public Double withdrawal_amount { get; set; }
    }
}
