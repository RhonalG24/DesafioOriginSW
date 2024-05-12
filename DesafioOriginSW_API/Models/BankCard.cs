using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.Models
{
    public class BankCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 id_bank_card { get; set; }
        [Required]
        public Int32 id_account { get; set; }
        
        [Required]
        [MaxLength(16), MinLength(16)]
        public String number { get; set; }
        
        [Required]
        [MaxLength(4), MinLength(4)]
        public String pin { get; set; }
        
        [Required]
        public Int32 id_card_state { get; set; }
        
        [Required]
        public DateOnly expiry_date { get; set; }

        [ForeignKey("id_account")]
        public Account account{ get; set; }
        
        [ForeignKey("id_card_state")]
        public CardState card_state { get; set; }
    }
}
