using DesafioOriginSW_API.Models.Errors;
using DesafioOriginSW_API.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using DesafioOriginSW_API.Models;

namespace DesafioOriginSW_API.Controllers
{
    public class ResultsControllerBase : ControllerBase
    {
        protected ActionResult<T> Ok<T>(IResult<T> result)
        {
            /*if (result.IsSuccess)
            {
                return Ok(result.Value);
            }*/
            if (result.IsSuccess)
            {
                return Ok(new APIResponse<T>(
                    result: result.Value,
                    isSuccessful: true,
                    statusCode: System.Net.HttpStatusCode.OK,
                    errorsMessage: null));
            }

            var error = result.Errors.First();

            if (error is RequestValidationError)
            {
                return Problem(detail: error.Message, statusCode: StatusCodes.Status400BadRequest);
            }

            if (error is NotFoundError)
                return Problem(detail: error.Message, statusCode: StatusCodes.Status404NotFound);

            if (error is BadRequestError)
                return Problem(detail: error.Message, statusCode: StatusCodes.Status400BadRequest);

            // Throw - because we've got an error we haven't accounted for
            //return Problem(detail: error.Message, statusCode: StatusCodes.Status500InternalServerError);
            throw new Exception(result.ToString());
        }

        protected BadRequestObjectResult ReturnModelStateErrors(ModelStateDictionary modelState)
        {
            var errors = modelState.Values
                                   .SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();

            var result = Result.Fail("Validation failed")
                               .WithErrors(errors);

            return BadRequest(result);
        }        
        
        protected Boolean IsModelStateValid(ModelStateDictionary modelState)
        {
            return modelState.IsValid;
        }
    }
}
