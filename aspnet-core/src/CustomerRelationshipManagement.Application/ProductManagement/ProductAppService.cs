using CustomerRelationshipManagement.ErrorCode;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace CustomerRelationshipManagement.ProductManagement
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IRepository<Product> productRepository;

        public ProductAppService(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }
        /// <summary>
        /// 新增产品
        /// </summary>
        /// <param name="createProduct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        [HttpPost]
        public async Task<ProductDtos> AddProduct(CreateUpdateProductDtos createProduct)
        {
            //将接收到的对象转换成实体
            var product = ObjectMapper.Map<CreateUpdateProductDtos, Product>(createProduct);
            //保存到数据库
            product = await productRepository.InsertAsync(product);
            //将数据库操作成功后的 Product 实体转换为 BookDto 对象
            return ObjectMapper.Map<Product, ProductDtos>(product);
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
                return ApiResult<ProductDtos>.Fail("产品不存在", ResultCode.Fail);
            }
            await productRepository.DeleteAsync(productdel);
            return ApiResult<ProductDtos>.Success(ObjectMapper.Map<Product, ProductDtos>(productdel), ResultCode.Success);

        }
        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="apiPaging"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 
        [HttpGet]
        public async Task<ApiResult<ApiPaging<ProductDtos>>> GetProduct([FromQuery] ApiPaging apiPaging)
        {
            try
            {
                var productlist = await productRepository.GetQueryableAsync();
                if (!string.IsNullOrEmpty(apiPaging.Keyword))
                {
                    productlist = productlist.Where(x => x.ProductCode.Contains(apiPaging.Keyword) || x.ProductBrand.Contains(apiPaging.Keyword));

                }
                var totalCount = productlist.Count();
                var totalPage = (int)Math.Ceiling((double)productlist.Count() / apiPaging.PageSize);
                var pagedProduct = productlist.OrderBy(x => x.Id).Skip((apiPaging.PageIndex - 1) * apiPaging.PageSize).Take(apiPaging.PageSize).ToList();
                return ApiResult<ApiPaging<ProductDtos>>.Success(new ApiPaging<ProductDtos>
                {
                    Data = ObjectMapper.Map<List<Product>, List<ProductDtos>>(pagedProduct),
                    TotalCount = totalCount,
                    TotalPage = totalPage,
                }, ResultCode.Success);

            }
            catch (Exception)
            {

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
                return ApiResult<ProductDtos>.Success(ObjectMapper.Map<Product, ProductDtos>(product), ResultCode.Success);
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
                return ApiResult<CreateUpdateProductDtos>.Success(ObjectMapper.Map<Product, CreateUpdateProductDtos>(productDto), ResultCode.Success);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
