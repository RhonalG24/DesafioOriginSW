using AutoMapper;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DesafioOriginSW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationTypeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IOperationTypeRepository _repo;
        private readonly IMapper _mapper;
        protected APIResponse _response;


        public OperationTypeController(ILogger<OperationTypeController> logger,
                                    IOperationTypeRepository repo,
                                    IMapper mapper)
        {
            _logger = logger;
            _repo = repo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllOperationTypes()
        {
            try
            {
                IEnumerable<OperationType> operationTypeList = await _repo.GetAll();
                _response.Result = operationTypeList;
                _response.IsSuccessful = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            } catch (Exception ex)
            {
                _logger.LogError("GetOperationTypes", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() }; 
                return _response;
            }

        }

        [HttpGet("{id}", Name = "GetOperationType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetOperationType(int id)
        {
            try
            {
                //_logger.LogInformation("Get all accounts");
                var operationTypeFiltered = await _repo.Get(x => x.id_operation_type == id);
                if (operationTypeFiltered == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);

                }
                _response.Result = operationTypeFiltered;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get operation", ex.Message);
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }

        }
    }
}
