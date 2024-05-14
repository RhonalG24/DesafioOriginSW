using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.DTO_s
{
    public class GetBalanceDTO
    {
        public Double balance { get; set; }

        [MaxLength(16), MinLength(16)]
        public String bank_card_number { get; set; }
        public DateOnly bank_card_expiry_date { get; set; }
    }
}
