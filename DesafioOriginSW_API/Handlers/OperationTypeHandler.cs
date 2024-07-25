using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Errors;
using DesafioOriginSW_API.Repository.IRepository;
using FluentResults;
using DesafioOriginSW_API.Models.Response;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using DesafioOriginSW_API.Models.Entities;

namespace DesafioOriginSW_API.Handlers
{
    public class OperationTypeHandler : IOperationTypeHandler
    {
        
        private readonly IOperationTypeRepository _operationTypeRepository;
        public OperationTypeHandler(IOperationTypeRepository operationTypeRepository)
        {
            _operationTypeRepository = operationTypeRepository ?? throw new ArgumentNullException(nameof(operationTypeRepository));
        }

        public async Task<Result<AllOperationTypesResponse>> GetAllOperationTypes()
        {

            AllOperationTypesResponse response = new AllOperationTypesResponse();
            response.OperationTypes = await _operationTypeRepository.GetAll();

            if (response.OperationTypes == null)
                return new NotFoundError("No se encontró ningún registro");

            return Result.Ok(response);

        }



        public async Task<Result<APIResponse<OperationType>>> GetOperationType(int id)
        {
            if (id < 0)
                return new RequestValidationError("El id ingresado no es válido.");
            //return new APIResponse<OperationType>(
            //        result: null, 
            //        isSuccessful: false, 
            //        statusCode: HttpStatusCode.BadRequest,
            //        errorsMessage: new List<string>{ "El id ingresado no es válido." });

            var operationTypeFiltered = await GetOperationTypeById(id);
            if (operationTypeFiltered == null)
                //    return new APIResponse<OperationType>(
                //        result: null,
                //        isSuccessful: false,
                //        statusCode: HttpStatusCode.NotFound,
                //        errorsMessage: new List<string> { "Id no existe." });
                return new NotFoundError("Id no existe.");

            return Result.Ok(new APIResponse<OperationType>(
                    result: operationTypeFiltered,
                    isSuccessful: true,
                    statusCode: HttpStatusCode.OK,
                    errorsMessage: null));   
            //return Result.Ok<OperationType?>(operationTypeFiltered);
    }

        private async Task<OperationType> GetOperationTypeById( int id )
        {
            return await _operationTypeRepository.Get(x => x.id_operation_type == id);
        }
    }
}
