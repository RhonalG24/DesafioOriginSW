using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioOriginSW_API.Models
{
    public class OperationType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 id_operation_type { get; set; }

        [Required]
        public String name { get; set; }
    }
}
