using DesafioOriginSW_API.Models;
using DesafioOriginSW_API.Models.Entities;
using DesafioOriginSW_API.Models.Response;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace DesafioOriginSW_API.Handlers.IHandler
{
    public interface IOperationTypeHandler
    {
        Task<Result<AllOperationTypesResponse>> GetAllOperationTypes();

        Task<Result<APIResponse<OperationType>>> GetOperationType(int id);

    }
}
