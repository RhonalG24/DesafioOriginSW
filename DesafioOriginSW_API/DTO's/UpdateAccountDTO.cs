using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.DTO_s
{
    public class UpdateAccountDTO
    {
        [Required]
        public Int32 id_account { get; set; }

        [Required]
        public Double balance { get; set; }
    }
}
