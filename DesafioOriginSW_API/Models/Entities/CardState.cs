using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.Models.Entities
{
    public class CardState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_card_state { get; set; }

        [Required]
        public string name { get; set; }
    }
}
