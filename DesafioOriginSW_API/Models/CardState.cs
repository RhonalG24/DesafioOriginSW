using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.Models
{
    public class CardState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 id_card_state { get; set; }
        
        [Required]
        public String name { get; set; }
    }
}
