using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.CustomerRegions;
using CustomerRelationshipManagement.CustomerProcess.Customers;
using CustomerRelationshipManagement.CustomerProcess.Customers.Helpers;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerContacts;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerRegions;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.ICustomerContacts;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.RBAC.Roles;
using CustomerRelationshipManagement.RBACDtos.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace CustomerRelationshipManagement.CustomerProcess.CustomerContacts
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class CustomerContactService : ApplicationService, ICustomerContactService
    {
        private readonly IRepository<CustomerContact> repository;
        private readonly IRepository<RoleInfo> rolerepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<CustomerRegion> customerRegionRepository;
        ILogger<CustomerContactService> logger;
        private readonly IDistributedCache<PageInfoCount<CustomerContactDto>> cache;

        public CustomerContactService(IRepository<CustomerContact> repository, IRepository<RoleInfo> rolerepository, IRepository<Customer> customerRepository, IRepository<CustomerRegion> customerRegionRepository, ILogger<CustomerContactService> logger, IDistributedCache<PageInfoCount<CustomerContactDto>> cache)
        {
            this.repository = repository;
            this.rolerepository = rolerepository;
            this.customerRepository = customerRepository;
            this.customerRegionRepository = customerRegionRepository;
            this.logger = logger;
            this.cache = cache;
        }

        /// <summary>
        /// 添加联系人信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public async Task<ApiResult<CustomerContactDto>> AddCustomerContact(CreateUpdateCustomerContactDto dto)
        {
            try
            {
                var customerContact = ObjectMapper.Map<CreateUpdateCustomerContactDto,CustomerContact>(dto);
                var list =await repository.InsertAsync(customerContact);
                return ApiResult<CustomerContactDto>.Success(ResultCode.Success, ObjectMapper.Map<CustomerContact, CustomerContactDto>(list));
            }
            catch (Exception ex)
            {
                logger.LogError("添加联系人信息出错！" + ex.Message);
                throw;
            }
        }
      
        /// <summary>
        /// 获取联系人详情信息
        /// </summary>
        /// <param name="id">要查询的联系人ID</param>
        /// <returns></returns>
        /// 
        [HttpGet]

        public async Task<ApiResult<CustomerContactDto>> GetCustomerContactById(Guid id)
        {
            try
            {
                var customerContact = await repository.GetAsync(x => x.Id == id);
                if (customerContact == null)
                {
                    return ApiResult<CustomerContactDto>.Fail("联系人信息不存在", ResultCode.NotFound);
                }
                return ApiResult<CustomerContactDto>.Success(ResultCode.Success, ObjectMapper.Map<CustomerContact, CustomerContactDto>(customerContact));

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 删除联系人信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult<CustomerContactDto>> DelCustomerContact(Guid id)
        {
            try
            {
                var customerContact = await repository.GetAsync(x => x.Id == id);
                if (customerContact == null)
                {
                    return ApiResult<CustomerContactDto>.Fail("联系人信息不存在", ResultCode.NotFound);
                }
                customerContact.IsDeleted = true;
                await repository.UpdateAsync(customerContact);
                return ApiResult<CustomerContactDto>.Success(ResultCode.Success, ObjectMapper.Map<CustomerContact, CustomerContactDto>(customerContact));
            }
            catch (Exception ex)
            {
               logger.LogError("删除联系人信息出错！" + ex.Message);
               throw;
            }
        }





        /// <summary>
        /// 修改联系人信息
        /// </summary>
        /// <param name="id">要修改的联系人ID</param>
        /// <param name="dto">联系人信息</param>
        /// <returns></returns>
        /// 
        [HttpPut]
        public async Task<ApiResult<CreateUpdateCustomerContactDto>> UpdCustomerContact(Guid id, CreateUpdateCustomerContactDto dto)
        {
            try
            {
                var customerContact = await repository.GetAsync(x => x.Id == id);
                if (customerContact == null)
                {
                    return ApiResult<CreateUpdateCustomerContactDto>.Fail("联系人信息不存在", ResultCode.NotFound);
                }
                var customerContactDto = ObjectMapper.Map(dto, customerContact);
                await repository.UpdateAsync(customerContactDto);
                return ApiResult<CreateUpdateCustomerContactDto>.Success(ResultCode.Success, ObjectMapper.Map<CustomerContact, CreateUpdateCustomerContactDto>(customerContactDto));

            }
            catch (Exception ex)
            {
                logger.LogError("修改联系人信息出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<ApiResult<List<CustomerDto>>> GetCustomerList()
        {
            try
            {
                var list = await customerRepository.GetQueryableAsync();
                var customer = list.Select(u=>new CustomerDto
                {
                    Id = u.Id,
                    CustomerName = u.CustomerName,
                }).ToList();
                return ApiResult<List<CustomerDto>>.Success(ResultCode.Success, customer);
            }
            catch (Exception ex)
            {
                logger.LogError("获取客户列表出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取联系人关系列表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]

        public async Task<ApiResult<List<CustomerRegionDto>>> GetContactRelationList()
        {
            try
            {
                var customerregionlist=await customerRegionRepository.GetQueryableAsync();
                var result = customerregionlist
                    .Select(u => new
                    {
                        IdString = u.Id.ToString(), // 转为 string
                        u.CustomerRegionName
                    })
                    .AsEnumerable() // 从数据库取出后处理 Guid
                    .Where(u => Guid.TryParse(u.IdString, out _)) // 过滤非法 Guid
                    .Select(u => new CustomerRegionDto
                    {
                        Id = Guid.Parse(u.IdString),
                        CustomerRegionName = u.CustomerRegionName
                    })
                    .ToList();
                return ApiResult<List<CustomerRegionDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception ex)
            {
                logger.LogError("获取客户列表出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<ApiResult<List<RoleDto>>> GetRoleDtoList()
        {
            try
            {
                var role=await rolerepository.GetQueryableAsync();
                var result = role.Select(u => new RoleDto
                {
                    Id = u.Id,
                    RoleName=u.RoleName,
                   
                }).ToList();
                return ApiResult<List<RoleDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 显示联系人列表信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public Task<ApiResult<PageInfoCount<CustomerContactDto>>> ShowCustomerContact([FromQuery] SearchCustomerContactDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
