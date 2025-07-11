﻿//using CustomerRelationshipManagement.DTOS.UploadFileDto;
using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.Customers.Helpers;
using CustomerRelationshipManagement.CXS.ProductManagement.Helpers;
using CustomerRelationshipManagement.Dtos.CrmContractDtos;
using CustomerRelationshipManagement.DTOS.CategoryMangamentDto;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using CustomerRelationshipManagement.DTOS.Export;
using CustomerRelationshipManagement.DTOS.ProductManagementDto;
using CustomerRelationshipManagement.Interfaces.IProductAppService;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.ProductCategory.Categorys;
using CustomerRelationshipManagement.ProductCategory.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using NPOI.POIFS.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using StackExchange.Redis;

namespace CustomerRelationshipManagement.CXS.ProductManagement
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IRepository<Product, Guid> productRepository;
        private readonly IRepository<Category, Guid> ctegoryRepository;
        public IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<ProductAppService> logger;

        private readonly IExportAppService exportAppService;
        private readonly IDistributedCache<PageInfoCount<ProductDtos>> cache;
        private readonly IConnectionMultiplexer connectionMultiplexer;


        public ProductAppService(IRepository<Product, Guid> productRepository, IHttpContextAccessor httpContextAccessor, ILogger<ProductAppService> logger, IRepository<Category, Guid> ctegoryRepository, IExportAppService exportAppService, IDistributedCache<PageInfoCount<ProductDtos>> cache, IConnectionMultiplexer connectionMultiplexer)
        {
            this.productRepository = productRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.ctegoryRepository = ctegoryRepository;
            this.exportAppService = exportAppService;
            this.cache = cache;
            this.connectionMultiplexer = connectionMultiplexer;
        }
        /// <summary>
        ///清除关于c:PageInfo,k:*的缓存
        /// </summary>
        /// <returns></returns>
        public async Task ClearAbpCacheAsync()
        {
            var endpoints = connectionMultiplexer.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = connectionMultiplexer.GetServer(endpoint);
                var keys = server.Keys(pattern: "c:PageInfo,k:*");
                foreach (var key in keys)
                {
                    await connectionMultiplexer.GetDatabase().KeyDeleteAsync(key);
                }
            }
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
            await ClearAbpCacheAsync(); //清除缓存
            return ApiResult<ProductDtos>.Success(ResultCode.Success, ObjectMapper.Map<Product, ProductDtos>(productdel));

            

        }
        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 
        [HttpGet]
        public async Task<ApiResult<PageInfoCount<ProductDtos>>> GetProduct([FromQuery] SearchProductDto dto)
        {
            try
            {
                string cachekey = ProductCachKeyHelper.ProductBuildHashKey(dto);

                var redislist = await cache.GetOrAddAsync(cachekey, async () =>
                {
                    var productlist = await productRepository.GetQueryableAsync();
                    var categorylist = await ctegoryRepository.GetQueryableAsync();
                    var query = from a in productlist
                                join b in categorylist on a.CategoryId equals b.Id into temp
                                from b in temp.DefaultIfEmpty()
                                select new ProductDtos
                                {
                                    Id = a.Id,
                                    CategoryId = a.CategoryId,
                                    CategoryName = b.CategoryName, // 这里做了判空
                                    ParentId = a.ParentId,
                                    ProductImageUrl = a.ProductImageUrl,
                                    ProductBrand = a.ProductBrand,
                                    ProductSupplier = a.ProductSupplier,
                                    ProductCode = a.ProductCode,
                                    ProductDescription = a.ProductDescription,
                                    SuggestedPrice = a.SuggestedPrice,
                                    ProductRemark = a.ProductRemark,
                                    ProductStatus = a.ProductStatus,
                                    DealPrice = a.DealPrice
                                };

                    query = query.WhereIf(!string.IsNullOrEmpty(dto.ProductCode), x => x.ProductCode.Contains(dto.ProductCode));

                    query = query.WhereIf(!string.IsNullOrEmpty(dto.ProductBrand), x => x.ProductBrand.Contains(dto.ProductBrand));

                    query = query.WhereIf(!string.IsNullOrEmpty(dto.ProductDescription), x => x.ProductDescription.Contains(dto.ProductDescription));

                    query = query.WhereIf(!string.IsNullOrEmpty(dto.ProductSupplier), x => x.ProductSupplier.Contains(dto.ProductSupplier));

                    query = query.WhereIf(!string.IsNullOrEmpty(dto.ProductImageUrl), x => x.ProductImageUrl.Contains(dto.ProductImageUrl));

                    query = query.WhereIf(dto.ProductStatus, x => x.ProductStatus == dto.ProductStatus);

                    query = query.WhereIf(dto.CategoryId != Guid.Empty, x => x.CategoryId == dto.CategoryId);

                    query = query.WhereIf(dto.SuggestedPrice != 0, x => x.SuggestedPrice == dto.SuggestedPrice);

                    query = query.WhereIf(dto.DealPrice != 0, x => x.DealPrice == dto.DealPrice);

                    query = query.WhereIf(dto.ProductRemark != null, x => x.ProductRemark.Contains(dto.ProductRemark));

                    query = query.WhereIf(dto.CategoryName != null, x => x.CategoryName.Contains(dto.CategoryName));
                    //用ABP框架的分页
                    var res = query.PageResult(dto.PageIndex, dto.PageSize);
                    //构建分页结果对象
                    return new PageInfoCount<ProductDtos>
                    {
                        TotalCount = res.RowCount,
                        PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / dto.PageSize),
                        Data = res.Queryable.ToList()
                    };
                }, () => new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(5)     //设置缓存过期时间为5分钟
                });
                return ApiResult<PageInfoCount<ProductDtos>>.Success(ResultCode.Success, redislist);



                ////abp分页
                //var querypaging = query.PageResult(dto.PageIndex, dto.PageSize);

                ////将数据通过映射转换
                //var productdto = ObjectMapper.Map<IList<Product>, IList<ProductDtos>>(querypaging.Queryable.ToList());

                //var productinfo = await productRepository.GetQueryableAsync();
                //var categoryinfo = await ctegoryRepository.GetQueryableAsync();

                //var pageInfo = new PageInfoCount<ProductDtos>
                //{

                //    TotalCount = querypaging.RowCount,
                //    PageCount = (int)MathCeiling(querypaging.RowCount * 1.0 / dto.PageSize),.
                //    Data = productdto
                //};
                //return ApiResult<PageInfoCount<ProductDtos>>.Success(ResultCode.Success, pageInfo);


                //分页
                //var querypaging = query.OrderByDescending(x => x.Id).Skip(dto.PageIndex).Take(dto.PageSize);
                ////将数据通过映射转换
                //var productdto = ObjectMapper.Map<IList<Product>, IList<ProductDtos>>(querypaging.ToList());
                //返回apiresult
                //return ApiResult<IList<ProductDtos>>.Success(ResultCode.Success, productdto);

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
                await ClearAbpCacheAsync();
                return ApiResult<CreateUpdateProductDtos>.Success(ResultCode.Success, ObjectMapper.Map<Product, CreateUpdateProductDtos>(productDto));
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 树形菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<CategoryTreeDtos>>> GetCategeryCascadeList()
        {
            try
            {
                var allCategories = await ctegoryRepository.GetListAsync();
                //递归构建树
                List<CategoryTreeDtos> BuildTree(Guid parentId)
                {
                    return allCategories
                        .Where(c => c.ParentId == parentId)
                        .Select(x => new CategoryTreeDtos
                        {
                            Id = x.Id,
                            CategoryName = x.CategoryName,
                            EnglishName = x.EnglishName,
                            Description = x.Description,
                            ParentId = x.ParentId,
                            Children = BuildTree(x.Id)
                        })
                        .ToList();
                }
                //假设根节点ParentId为Guid.Empty
                var tree = BuildTree(Guid.Empty);
                return ApiResult<List<CategoryTreeDtos>>.Success(ResultCode.Success, tree);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 导出所有产品
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IRemoteStreamContent> ExportAllProductCategoryToAsync()
        {
            var list = await productRepository.GetListAsync();
            var categorylist = await ctegoryRepository.GetListAsync();
            var category = from a in list
                           join b in categorylist on a.CategoryId equals b.Id into temp
                           from b in temp.DefaultIfEmpty()
                           select new ProductDtos
                           {
                               Id = a.Id,
                               CategoryId = a.CategoryId,
                               CategoryName = b != null ? b.CategoryName : "", // 这里做了判空
                               ParentId = a.ParentId,
                               ProductImageUrl = a.ProductImageUrl,
                               ProductBrand = a.ProductBrand,
                               ProductSupplier = a.ProductSupplier,
                               ProductCode = a.ProductCode,
                               ProductDescription = a.ProductDescription,
                               SuggestedPrice = a.SuggestedPrice,
                               ProductRemark = a.ProductRemark,
                               ProductStatus = a.ProductStatus,
                               DealPrice = a.DealPrice
                           };

            var exportData = new ExportDataDto<ProductDtos>
            {
                FileName = "产品管理",
                Items = category.ToList(),
                ColumnMappings = new Dictionary<string, string>
                {
                    { "Id", "产品ID" },
                    { "CategoryId", "分类ID" },
                    { "CategoryName", "分类名称" },
                    { "ParentId", "父级ID" },
                    { "ProductImageUrl", "产品图片" },
                    { "ProductBrand", "品牌" },
                    { "ProductSupplier", "供应商" },
                    { "ProductCode", "产品编号" },
                    { "ProductDescription", "产品描述" },
                    { "SuggestedPrice", "建议售价" },
                    { "ProductRemark", "备注" },
                    { "ProductStatus", "产品状态" },
                    { "DealPrice", "成交价" }
                }
            };
            return await exportAppService.ExportToExcelAsync(exportData);
        }

        /// <summary>
        /// 修改产品状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        /// 
        [HttpPut]
        public async Task<ApiResult<ProductDtos>> UpdProductState(Guid id, bool state)
        {
            try
            {
                var updproductState= await productRepository.GetAsync(x => x.Id == id);
                if(updproductState == null)
                {
                    return ApiResult<ProductDtos>.Fail("产品不存在", ResultCode.Fail);
                }
                updproductState.ProductStatus = state;
                await productRepository.UpdateAsync(updproductState);
                await ClearAbpCacheAsync();
                return ApiResult<ProductDtos>.Success(ResultCode.Success,ObjectMapper.Map<Product, ProductDtos>(updproductState));
            }
            catch (Exception ex)
            {
                logger.LogError("修改产品状态信息出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<ApiResult<List<CategoryDtosList>>> GetCategoryDtoList()
        {
            try
            {
                var list = await ctegoryRepository.GetQueryableAsync();
                var category = list.Select(u => new CategoryDtosList
                {
                    Id = u.Id,
                    CategoryName = u.CategoryName,
                }).ToList();
                return ApiResult<List<CategoryDtosList>>.Success(ResultCode.Success, category);
            }
            catch (Exception ex)
            {
                logger.LogError("获取产品列表出错！" + ex.Message);
                throw;
            }
        }
    }

}




