using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioOriginSW_API.Models
{
    public class Operation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 id_operation { get; set; }

        [Required]
        public Int32 id_bank_card { get; set; }

        [Required]
        public Int32 id_operation_type { get; set; }

        [Required]
        public DateTime date { get; set; }

        public Double? amount { get; set; }

        [ForeignKey("id_bank_card")]
        public BankCard bank_card { get; set; }

        [ForeignKey("id_operation_type")]
        public OperationType operation_type { get; set; }
    }
}
