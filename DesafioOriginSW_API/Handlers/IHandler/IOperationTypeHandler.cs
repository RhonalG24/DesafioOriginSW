using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Entities;
using DesafioOriginSW_API.Models.Responses.OperationType;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace DesafioOriginSW_API.Handlers.IHandler
{
    public interface IOperationTypeHandler
    {
        Task<Result<GetAllOperationTypesResponse>> GetAllOperationTypes();

        Task<Result<GetOperationTypeResponse>> GetOperationType(int id);

    }
}
