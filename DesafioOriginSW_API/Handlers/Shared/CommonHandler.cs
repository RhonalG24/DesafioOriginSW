using DesafioOriginSW_API.Models;
using System.Net;

namespace DesafioOriginSW_API.Handlers.Shared
{
    public class CommonHandler<T>
    {
        public static APIResponse<T> ReturnSuccessfulResponse(T result)
        {
            return new APIResponse<T>(
                result: result,
                isSuccessful: true,
                statusCode: HttpStatusCode.OK,
                errorsMessage: null);
        }
    }
}
