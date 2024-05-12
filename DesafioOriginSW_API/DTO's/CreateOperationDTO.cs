using DesafioOriginSW_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioOriginSW_API.DTO_s
{
    public class CreateOperationDTO
    {

        [Required]
        public Int32 id_bank_card { get; set; }

        [Required]
        public Int32 id_operation_type { get; set; }

        public Double? amount { get; set; }

    }
}
