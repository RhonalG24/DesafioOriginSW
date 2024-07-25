using DesafioOriginSW_API.Handlers.IHandler;
using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Errors;
using DesafioOriginSW_API.Repository.IRepository;
using FluentResults;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using DesafioOriginSW_API.Models.Entities;
using DesafioOriginSW_API.Models.Responses.OperationType;
using AutoMapper;

namespace DesafioOriginSW_API.Handlers
{
    public class OperationTypeHandler : IOperationTypeHandler
    {
        
        private readonly IOperationTypeRepository _operationTypeRepository;
        private readonly IMapper _mapper;
        public OperationTypeHandler(IOperationTypeRepository operationTypeRepository, IMapper mapper)
        {
            _operationTypeRepository = operationTypeRepository ?? throw new ArgumentNullException(nameof(operationTypeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<GetAllOperationTypesResponse>> GetAllOperationTypes()
        {

            GetAllOperationTypesResponse operationTypesList = new GetAllOperationTypesResponse();
            operationTypesList.OperationTypes = await _operationTypeRepository.GetAll();

            if (operationTypesList.OperationTypes == null)
                return new NotFoundError("No se encontró ningún registro");
            
            return Result.Ok(operationTypesList);
        }

        public async Task<Result<GetOperationTypeResponse>> GetOperationType(int id)
        {
            if (id < 0)
                return new RequestValidationError("El id ingresado no es válido.");


            var operationTypeFiltered = await GetOperationTypeById(id);
            if (operationTypeFiltered == null)
                return new NotFoundError("Id no existe.");

            var response = _mapper.Map<GetOperationTypeResponse>(operationTypeFiltered);

            return Result.Ok(response);   
        }

        private async Task<OperationType> GetOperationTypeById( int id )
        {
            return await _operationTypeRepository.Get(x => x.id_operation_type == id);
        }
    }
}
