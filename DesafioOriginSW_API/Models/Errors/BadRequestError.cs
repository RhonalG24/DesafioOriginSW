using FluentResults;

namespace DesafioOriginSW_API.Models.Errors
{
    public class BadRequestError : Error
    {
        public BadRequestError(String message) : base(message)
        { }
    }
}
