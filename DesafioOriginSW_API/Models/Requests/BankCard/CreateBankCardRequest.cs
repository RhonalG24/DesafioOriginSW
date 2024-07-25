using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.Models.Requests.BankCard
{
    public class CreateBankCardRequest
    {

        [Required]
        public Int32 id_account { get; set; }

        [Required]
        [MaxLength(16), MinLength(16)]
        public String number { get; set; }

        [Required]
        [MaxLength(4), MinLength(4)]
        public String pin { get; set; }
    }
}
