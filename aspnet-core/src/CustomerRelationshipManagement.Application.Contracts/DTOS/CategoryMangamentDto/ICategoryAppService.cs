using CustomerRelationshipManagement.ErrorCode;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.DTOS.CategoryMangamentDto
{
    public interface ICategoryAppService:IApplicationService
    {
        /// <summary>
        /// 添加产品类型
        /// </summary>
        /// <param name="createCatgory"></param>
        /// <returns></returns>
        Task<CategoryDtos> AddCategory(CreateUpdateCategoryDtos createCategory);
        /// <summary>
        /// 显示产品类型
        /// </summary>
        /// <param name="apiPaging"></param>
        /// <returns></returns>
        Task<ApiResult<ApiPaging<CategoryDtos>>> GetCategory([FromQuery] ApiPaging apiPaging);
        /// <summary>
        /// 获取产品类型详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<CategoryDtos>> GetCategoryId(Guid id);
        /// <summary>
        /// 删除产品类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<CategoryDtos>> DeletedCategory(Guid id);
        /// <summary>
        /// 修改产品类型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateCatgory"></param>
        /// <returns></returns>
        Task<ApiResult<CreateUpdateCategoryDtos>> UdpateCategory(Guid id, CreateUpdateCategoryDtos updateCategory);


    }
}
