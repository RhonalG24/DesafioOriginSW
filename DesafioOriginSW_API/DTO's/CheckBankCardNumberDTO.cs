using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.DTO_s
{
    public class CheckBankCardNumberDTO
    {
        [Required]
        [MaxLength(16), MinLength(16)]
        public String number { get; set; }
    }
}
