using DesafioOriginSW_API.Models.Entities;

namespace DesafioOriginSW_API.Models
{
    public class GetAllAccountsResponse
    {
        public IEnumerable<Account>? Accounts { get; set; }
    }
}
