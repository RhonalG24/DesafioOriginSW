using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.Models.Entities
{
    public class BankCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_bank_card { get; set; }
        [Required]
        public int id_account { get; set; }

        [Required]
        [MaxLength(16), MinLength(16)]
        public string number { get; set; }

        [Required]
        [MaxLength(4), MinLength(4)]
        public string pin { get; set; }

        [Required]
        public int id_card_state { get; set; }

        [Required]
        public DateOnly expiry_date { get; set; }

        public int failed_attempts { get; set; }

        [ForeignKey("id_account")]
        public Account account { get; set; }

        [ForeignKey("id_card_state")]
        public CardState card_state { get; set; }
    }
}
