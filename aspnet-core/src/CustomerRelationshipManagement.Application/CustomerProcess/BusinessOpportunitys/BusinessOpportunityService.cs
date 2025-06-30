using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.Customers;
using CustomerRelationshipManagement.CustomerProcess.Prioritys;
using CustomerRelationshipManagement.CustomerProcess.SalesProgresses;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.BusinessOpportunitys;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Levels;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Prioritys;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.SalesProgresses;
using CustomerRelationshipManagement.DTOS.ProductManagementDto;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.IBusinessOpportunitys;
using CustomerRelationshipManagement.ProductCategory.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CustomerProcess.BusinessOpportunitys
{
    /// <summary>
    /// 商机业务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class BusinessOpportunityService : ApplicationService, IBusinessOpportunityService
    {
        private readonly IRepository<BusinessOpportunity> businessopportunityrepository;
        private readonly IRepository<Customer> customerrepository;
        private readonly IRepository<Priority> priorityrepository;
        private readonly IRepository<SalesProgress> salesprogressrepository;
        private readonly IRepository<Product> productrepository;
        private readonly ILogger<BusinessOpportunityService> logger;
        public BusinessOpportunityService(IRepository<BusinessOpportunity> businessopportunityrepository, ILogger<BusinessOpportunityService> logger, IRepository<Customer> customerrepository, IRepository<Priority> priorityrepository, IRepository<SalesProgress> salesprogressrepository, IRepository<Product> productrepository)
        {
            this.businessopportunityrepository = businessopportunityrepository;
            this.logger = logger;
            this.customerrepository = customerrepository;
            this.priorityrepository = priorityrepository;
            this.salesprogressrepository = salesprogressrepository;
            this.productrepository = productrepository;
        }

        /// <summary>
        /// 添加商机
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<BusinessOpportunityDto>> AddBusinessOpportunity(CreateUpdateBusinessOpportunityDto dto)
        {
            try
            {
                var businessopportunity = ObjectMapper.Map<CreateUpdateBusinessOpportunityDto, BusinessOpportunity>(dto);
                var list = await businessopportunityrepository.InsertAsync(businessopportunity);
                return ApiResult<BusinessOpportunityDto>.Success(ResultCode.Success, ObjectMapper.Map<BusinessOpportunity, BusinessOpportunityDto>(list));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "添加商机失败");
                return ApiResult<BusinessOpportunityDto>.Fail("添加商机失败", ResultCode.Fail);
            }
        }


        /// <summary>
        /// 获取客户下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<CustomerSimpleDto>>> GetCustomerSelectList()
        {
            try
            {
                var customerList = await customerrepository.GetQueryableAsync();
                var result = customerList
                    .Select(u => new CustomerSimpleDto
                    {
                        Id = u.Id,
                        CustomerCode = u.CustomerCode,
                        CustomerName = u.CustomerName,
                        CustomerPhone = u.CustomerPhone,
                        CreationTime = u.CreationTime
                    })
                    .ToList();
                return ApiResult<List<CustomerSimpleDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取商机优先级下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<PriorityDto>>> GetPrioritySelectList()
        {
            try
            {
                var priorityList = await priorityrepository.GetListAsync();
                var result = priorityList
                    .Select(u => new PriorityDto
                    {
                        Id = u.Id,
                        PriorityName = u.PriorityName
                    })
                    .ToList();
                return ApiResult<List<PriorityDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取商机销售进度下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<SalesProgressDto>>> GetSalesProgressSelectList()
        {
            try
            {
                var salesProgressList = await salesprogressrepository.GetListAsync();
                var result = salesProgressList
                    .Select(u => new SalesProgressDto
                    {
                        Id = u.Id,
                        SalesProgressName = u.SalesProgressName
                    })
                    .ToList();
                return ApiResult<List<SalesProgressDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取产品下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<ProductDtos>>> GetProductSelectList()
        {
            try
            {
                // 从仓储获取所有产品列表
                var productList = (await productrepository.GetListAsync())
                    .Where(u => Guid.TryParse(u.Id.ToString(), out _)) // 只保留合法GUID
                    .ToList();

                // 过滤掉Id不是合法Guid的产品，并映射为ProductDtos
                var result = productList
                    .Where(u => u.Id != Guid.Empty) // 过滤掉空Guid
                    .Select(u => new ProductDtos
                    {
                        Id = u.Id,
                        CategoryId = u.CategoryId,
                        ProductImageUrl = u.ProductImageUrl,
                        ProductBrand = u.ProductBrand,
                        ProductCode = u.ProductCode,
                        DealPrice = u.DealPrice
                    })
                    .ToList();

                return ApiResult<List<ProductDtos>>.Success(ResultCode.Success, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "获取产品下拉列表失败");
                return ApiResult<List<ProductDtos>>.Fail("获取产品下拉列表失败", ResultCode.Fail);
            }
        }
    }
}
