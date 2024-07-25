using FluentResults;
namespace DesafioOriginSW_API.Models.Errors
{
    public class NotFoundError : Error
    {
        public NotFoundError(String message) : base( message ) 
        { 
        }
    }
}
