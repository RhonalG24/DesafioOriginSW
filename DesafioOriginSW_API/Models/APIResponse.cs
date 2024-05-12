using System.Net;

namespace DesafioOriginSW_API.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public List<string>? ErrorsMessage { get; set; }
        public object? Result { get; set; }
    }
}

