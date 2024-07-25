using DesafioOriginSW_API.Models.Entities;

namespace DesafioOriginSW_API.Models.Response
{
    public class AllOperationTypesResponse
    {

        //public AllOperationTypesResponse() { }
        //public AllOperationTypesResponse(IEnumerable<OperationType>? operationTypes, APIResponse responseDetails)
        //{
        //    OperationTypes = operationTypes;
        //    ResponseDetails = responseDetails;

        //    if ( responseDetails.Result == null && operationTypes != null )
        //        responseDetails.Result = operationTypes;
        //}

        public IEnumerable<OperationType>? OperationTypes { get; set; }
        //public APIResponse ResponseDetails { get; set; } = new APIResponse();
    }
}
