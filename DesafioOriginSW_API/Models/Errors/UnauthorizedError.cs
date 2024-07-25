using FluentResults;

namespace DesafioOriginSW_API.Models.Errors
{
    public class UnauthorizedError : Error
    {
        public UnauthorizedError(string message) : base(message) { }
    }
}
