using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.DTO_s
{
    public class BankCardPinDTO
    {
        [Required]
        public Int32 id_bank_card { get; set; }

        [Required]
        [MaxLength(4), MinLength(4)]
        public String pin { get; set; }

    }
}
