using FluentResults;

namespace DesafioOriginSW_API.Models.Errors
{
    public class RequestValidationError : Error
    {
        public RequestValidationError(string message) : base(message)
        { 
        }
    }
}
