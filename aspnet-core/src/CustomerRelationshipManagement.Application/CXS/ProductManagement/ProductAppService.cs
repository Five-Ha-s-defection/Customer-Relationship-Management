//using CustomerRelationshipManagement.DTOS.UploadFileDto;
using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.ProductManagementDto;
using CustomerRelationshipManagement.Interfaces.IProductAppService;
using CustomerRelationshipManagement.ProductCategory.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CXS.ProductManagement
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IRepository<Product, Guid> productRepository;
        public IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<ProductAppService> logger;

        public ProductAppService(IRepository<Product, Guid> productRepository, IHttpContextAccessor httpContextAccessor, ILogger<ProductAppService> logger)
        {
            this.productRepository = productRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }
        /// <summary>
        /// 新增产品
        /// </summary>
        /// <param name="createProduct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>


        public async Task<ApiResult> AddProduct(CreateUpdateProductDtos createProduct)
        {
            //转换要添加的合同表数据
            var crmcontract = ObjectMapper.Map<CreateUpdateProductDtos, Product>(createProduct);

            //执行插入的数据的操作
            var result = await productRepository.InsertAsync(crmcontract);

            //返回统一返回值
            return ApiResult.Success(ResultCode.Success);

        }
        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 
        [HttpDelete]
        public async Task<ApiResult<ProductDtos>> DeletedProduct(Guid id)
        {
            var productdel = await productRepository.GetAsync(x => x.Id == id);
            if (productdel == null)
            {
                return ApiResult<ProductDtos>.Fail("未找到删除的线索", ResultCode.NotFound);
            }
            productdel.IsDeleted = true;
            await productRepository.UpdateAsync(productdel);
            return ApiResult<ProductDtos>.Success(ResultCode.Success, ObjectMapper.Map<Product, ProductDtos>(productdel));

        }
        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="apiPaging"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 
        [HttpGet]
        public async Task<ApiResult<IList<ProductDtos>>> GetProduct([FromQuery] SearchProductDto dto)
        {
            try
            {
                var query = await productRepository.GetQueryableAsync();
                //分页
                var querypaging = query.OrderByDescending(x => x.Id).Skip(dto.PageIndex).Take(dto.PageSize);
                //将数据通过映射转换
                var productdto = ObjectMapper.Map<IList<Product>, IList<ProductDtos>>(querypaging.ToList());
                //返回apiresult
                return ApiResult<IList<ProductDtos>>.Success(ResultCode.Success, productdto);

            }
            catch (Exception ex)
            {
                logger.LogError("产品分页显示或查询出错 " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<ApiResult<ProductDtos>> GetProductId(Guid id)
        {
            try
            {
                var product = await productRepository.GetAsync(x => x.Id == id);
                if (product == null)
                {
                    return ApiResult<ProductDtos>.Fail("未找到该数据", ResultCode.Fail);
                }
                return ApiResult<ProductDtos>.Success(ResultCode.Success, ObjectMapper.Map<Product, ProductDtos>(product));
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 修改产品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateProduct"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult<CreateUpdateProductDtos>> UpdateProduct(Guid id, CreateUpdateProductDtos updateProduct)
        {
            try
            {
                var product = await productRepository.GetAsync(x => x.Id == id);
                if (product == null)
                {
                    return ApiResult<CreateUpdateProductDtos>.Fail("产品不存在", ResultCode.NotFound);
                }
                var productDto = ObjectMapper.Map(updateProduct, product);
                await productRepository.UpdateAsync(productDto);
                return ApiResult<CreateUpdateProductDtos>.Success(ResultCode.Success, ObjectMapper.Map<Product, CreateUpdateProductDtos>(productDto));
            }
            catch (Exception)
            {

                throw;
            }
        }





    }
}
