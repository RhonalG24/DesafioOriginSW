using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.DTO_s
{
    public class CreateBankCardDTO
    {
        public Int32 id_account { get; set; }

        [MaxLength(16), MinLength(16)]
        public String number { get; set; }

        [MaxLength(4), MinLength(4)]
        public String pin { get; set; }
    }
}
