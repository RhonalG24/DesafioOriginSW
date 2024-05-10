using DesafioOriginSW_API.Data;
using DesafioOriginSW_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DesafioOriginSW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationTypeController : ControllerBase
    {
        private readonly AppDbContext _db;

        public OperationTypeController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OperationType>> GetOperationTypes()
        {
            return Ok(_db.operation_type.ToList());
        }
    }
}
