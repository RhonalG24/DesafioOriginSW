using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.DTO_s
{
    public class CheckBankCardNumberRespondDTO
    {
        [Required]
        public Int32 id_bank_card { get; set; }

        [Required]
        [MaxLength(16), MinLength(16)]
        public String number { get; set; }
    }
}
