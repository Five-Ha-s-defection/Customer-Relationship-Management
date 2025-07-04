using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.BusinessOpportunitys;
using CustomerRelationshipManagement.CustomerProcess.Clues;
using CustomerRelationshipManagement.CustomerProcess.Customers;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.ContactCommunications;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.IContactCommunications;
using CustomerRelationshipManagement.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace CustomerRelationshipManagement.CustomerProcess.ContactCommunications
{
    /// <summary>
    /// 联系沟通服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class ContactCommunicationService:ApplicationService,IContactCommunicationService
    {
        private readonly IRepository<ContactCommunication> contactCommunicationRepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Clue> cluerepository;
        private readonly IRepository<BusinessOpportunity> businessopportunityrepository;
        private readonly ILogger<ContactCommunicationService> logger;
        private readonly IDistributedCache<PageInfoCount<ContactCommunicationDto>> cache;
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public ContactCommunicationService(IRepository<ContactCommunication> contactCommunicationRepository, IRepository<Customer> customerRepository, IRepository<Clue> cluerepository, IRepository<BusinessOpportunity> businessopportunityrepository, ILogger<ContactCommunicationService> logger, IDistributedCache<PageInfoCount<ContactCommunicationDto>> cache, IConnectionMultiplexer connectionMultiplexer)
        {
            this.contactCommunicationRepository = contactCommunicationRepository;
            this.customerRepository = customerRepository;
            this.cluerepository = cluerepository;
            this.businessopportunityrepository = businessopportunityrepository;
            this.logger = logger;
            this.cache = cache;
            this.connectionMultiplexer = connectionMultiplexer;
        }

        /// <summary>
        /// 清楚关于c:PageInfo,k的所有信息
        /// </summary>
        /// <returns></returns>
        public async Task ClearAbpCacheAsync()
        {
            var endpoints = connectionMultiplexer.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = connectionMultiplexer.GetServer(endpoint);
                var keys = server.Keys(pattern: "c:PageInfo,k:*");//填写自己的缓存前缀
                foreach (var key in keys)
                {
                    await connectionMultiplexer.GetDatabase().KeyDeleteAsync(key);
                }
            }
        }

        /// <summary>
        /// 添加联系沟通
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<ContactCommunicationDto>> AddContactCommunication(CreateUpdateContactCommunicationDto dto)
        {
            try
            {
                // 只能选择客户或线索中的一个，且必须选择一个
                if ((dto.CustomerId == null && dto.ClueId == null && dto.BusinessOpportunityId==null) || (dto.CustomerId != null && dto.ClueId != null && dto.BusinessOpportunityId!=null))
                {
                    return ApiResult<ContactCommunicationDto>.Fail("只能选择客户、线索、商机中的一个，且必须选择一个", ResultCode.Fail);
                }
                // 将前端传来的DTO映射为数据库实体ContactCommunication
                var ContactCommunication = ObjectMapper.Map<CreateUpdateContactCommunicationDto, ContactCommunication>(dto);
                // 插入客户数据到数据库
                var list = await contactCommunicationRepository.InsertAsync(ContactCommunication);
                //清除缓存，确保数据一致性
                await ClearAbpCacheAsync();
                // 返回插入后的客户信息（DTO）
                return ApiResult<ContactCommunicationDto>.Success(ResultCode.Success, ObjectMapper.Map<ContactCommunication, ContactCommunicationDto>(list));
            }
            catch (Exception ex)
            {
                // 记录异常日志
                logger.LogError("添加客户信息出错！" + ex.Message);
                throw;
            }
        }

        //[HttpGet]
        //public async Task<ApiResult<PageInfoCount<ContactCommunicationDto>>> GetContactCommunicationList([FromQuery])
        //{
        //}
    }
}
