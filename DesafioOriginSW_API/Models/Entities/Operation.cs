using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioOriginSW_API.Models.Entities
{
    public class Operation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_operation { get; set; }

        [Required]
        public int id_bank_card { get; set; }

        [Required]
        public int id_operation_type { get; set; }

        [Required]
        public DateTime date { get; set; }

        public double? amount { get; set; }

        [ForeignKey("id_bank_card")]
        public BankCard bank_card { get; set; }

        [ForeignKey("id_operation_type")]
        public OperationType operation_type { get; set; }
    }
}
