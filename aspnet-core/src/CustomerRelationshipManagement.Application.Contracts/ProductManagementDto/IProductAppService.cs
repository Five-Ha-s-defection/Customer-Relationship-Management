using CustomerRelationshipManagement.ErrorCode;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;


namespace CustomerRelationshipManagement.ProductManagement
{
    public interface IProductAppService : IApplicationService
    {
        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="createProduct">属相</param>
        /// <returns></returns>
        Task<ProductDtos> AddProduct(CreateUpdateProductDtos createProduct);
        /// <summary>
        /// 显示产品
        /// </summary>
        /// <param name="apiPaging"></param>
        /// <returns></returns>
        Task<ApiResult<ApiPaging<ProductDtos>>> GetProduct([FromQuery] ApiPaging apiPaging);
        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<ProductDtos>> GetProductId(Guid id);
        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<ProductDtos>> DeletedProduct(Guid id);
        /// <summary>
        /// 修改产品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateProduct"></param>
        /// <returns></returns>
        Task<ApiResult<CreateUpdateProductDtos>> UpdateProduct
            (Guid id, CreateUpdateProductDtos updateProduct);


    }
}
