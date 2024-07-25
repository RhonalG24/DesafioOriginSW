using System.Net;

namespace DesafioOriginSW_API.Models
{
    public class APIResponse<T>
    {
        public APIResponse() { }
        public APIResponse(T? result, bool isSuccessful, HttpStatusCode statusCode, List<string>? errorsMessage)
        {
            Status = statusCode;
            IsSuccessful = isSuccessful;
            Detail = errorsMessage;
            Result = result;
        }

        public HttpStatusCode Status { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public List<string>? Detail { get; set; }
        public T? Result { get; set; }
    }
}

