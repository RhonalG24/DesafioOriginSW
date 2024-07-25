using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Entities;
using DesafioOriginSW_API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace DesafioOriginSW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationTypeController : ResultsControllerBase
    {
        private readonly IOperationTypeHandler _operationTypeHandler;

        public OperationTypeController(
            IOperationTypeHandler operationTypeHandler)
        {
            _operationTypeHandler = operationTypeHandler ?? throw new ArgumentNullException(nameof(operationTypeHandler));
        }

        #region [HttpGet]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        #endregion
        public async Task<ActionResult<AllOperationTypesResponse>> GetAllOperationTypes()
        {
            var operationTypes = await _operationTypeHandler.GetAllOperationTypes();
            return Ok(operationTypes);

        }

        //[HttpGet("{id}", Name = "GetOperationType")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<APIResponse>> GetOperationType(int id)
        //{
        //    try
        //    {
        //        //_logger.LogInformation("Get all accounts");
        //        var operationTypeFiltered = await _repo.Get(x => x.id_operation_type == id);
        //        if (operationTypeFiltered == null)
        //        {
        //            _response.IsSuccessful = false;
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_response);

        //        }
        //        _response.Result = operationTypeFiltered;
        //        _response.StatusCode = HttpStatusCode.OK;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Get operation", ex.Message);
        //        _response.IsSuccessful = false;
        //        _response.StatusCode = HttpStatusCode.InternalServerError;
        //        _response.ErrorsMessage = new List<string>() { ex.ToString() };
        //        return _response;
        //    }

        //}

        #region [HttpGet("{id}", Name = "GetOperationType")]
        [HttpGet("{id}", Name = "GetOperationType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        #endregion
        public async Task<ActionResult<APIResponse<OperationType>>> GetOperationType(int id)
        {
            var operationType = await _operationTypeHandler.GetOperationType(id);
            return Ok(operationType);
        }
    }
}
