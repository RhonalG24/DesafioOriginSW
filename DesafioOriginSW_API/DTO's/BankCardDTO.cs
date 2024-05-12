using DesafioOriginSW_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.DTO_s
{
    public class BankCardDTO
    {
        public Int32 id_bank_card { get; set; }
        public Int32 id_account { get; set; } 

        [MaxLength(16), MinLength(16)]
        public String number { get; set; }

        [MaxLength(4), MinLength(4)]
        public String pin { get; set; }
        public Int32 id_card_state { get; set; }
        public DateOnly expiry_date { get; set; }
    }

}
