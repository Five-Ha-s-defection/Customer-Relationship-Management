using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CategoryMangamentDto;
using CustomerRelationshipManagement.DTOS.ProductManagementDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;


namespace CustomerRelationshipManagement.Interfaces.IProductAppService
{
    public interface IProductAppService : IApplicationService
    {

        Task<ApiResult<List<CategoryDtos>>> GetCategeryCascadeList();

        Task<ApiResult<ProductDtos>> UpdProductState(Guid id, bool state);
    }
}
