using System.ComponentModel.DataAnnotations;

namespace DesafioOriginSW_API.DTO_s
{
    public class AccountDTO
    {
        [Required]  
        public Int32 id_account { get; set; }

        [Required]
        public Double balance { get; set; }
    }
}
