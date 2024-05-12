using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioOriginSW_API.Models
{
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 id_account { get; set; }

        [Required]
        public Double balance { get; set; }
    }
}
