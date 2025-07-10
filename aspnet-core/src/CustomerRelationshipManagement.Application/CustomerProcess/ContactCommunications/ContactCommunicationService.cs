using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.ContactCommunications;
using CustomerRelationshipManagement.CustomerProcess.BusinessOpportunitys;
using CustomerRelationshipManagement.CustomerProcess.Clues;
using CustomerRelationshipManagement.CustomerProcess.CommunicationTypes;
using CustomerRelationshipManagement.CustomerProcess.ContactCommunications.Helpers;
using CustomerRelationshipManagement.CustomerProcess.CustomerContacts;
using CustomerRelationshipManagement.CustomerProcess.Customers;
using CustomerRelationshipManagement.CustomerProcess.CustomReplys;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CommunicationTypes;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.ContactCommunications;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerContacts;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomReplys;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Industrys;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.IContactCommunications;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.RBAC.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

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
        private readonly IRepository<UserInfo> userRepository;
        private readonly IRepository<CommunicationType> communicationTypeRepository;
        private readonly IRepository<CustomReply> replyRepository;
        private readonly ILogger<ContactCommunicationService> logger;
        private readonly IDistributedCache<PageInfoCount<ContactCommunicationDto>> cache;
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public ContactCommunicationService(IRepository<ContactCommunication> contactCommunicationRepository, IRepository<Customer> customerRepository, IRepository<Clue> cluerepository, IRepository<BusinessOpportunity> businessopportunityrepository, ILogger<ContactCommunicationService> logger, IDistributedCache<PageInfoCount<ContactCommunicationDto>> cache, IConnectionMultiplexer connectionMultiplexer, IRepository<CommunicationType> communicationTypeRepository, IRepository<UserInfo> userRepository, IRepository<CustomReply> replyRepository)
        {
            this.contactCommunicationRepository = contactCommunicationRepository;
            this.customerRepository = customerRepository;
            this.cluerepository = cluerepository;
            this.businessopportunityrepository = businessopportunityrepository;
            this.logger = logger;
            this.cache = cache;
            this.connectionMultiplexer = connectionMultiplexer;
            this.communicationTypeRepository = communicationTypeRepository;
            this.userRepository = userRepository;
            this.replyRepository = replyRepository;
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

        /// <summary>
        /// 显示联系沟通列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<PageInfoCount<ContactCommunicationDto>>> GetContactCommunicationList([FromQuery]SearchContactCommunicationDto dto)
        {
            try
            {
                //缓存key(动态分页查询不同的key)
                string cacheKey = CuntactCommunicationCacheKeyHelper.BuildReadableKey(dto);
                //使用Redis缓存获取或添加数据
                var redislist = await cache.GetOrAddAsync(cacheKey, async () =>
                {
                    var customerlist = await customerRepository.GetQueryableAsync();
                    var cluelist = await cluerepository.GetQueryableAsync();
                    var userlist = await userRepository.GetQueryableAsync();
                    var replylist = await replyRepository.GetQueryableAsync();
                    var businessopportunitylist = await businessopportunityrepository.GetQueryableAsync();
                    var communicationtypelist = await communicationTypeRepository.GetQueryableAsync();
                    var contactCommunicationlist = await contactCommunicationRepository.GetQueryableAsync();
                    var list = from communication in contactCommunicationlist
                               join cus in customerlist on communication.CustomerId equals cus.Id into cusGroup
                               from cus in cusGroup.DefaultIfEmpty()
                               join clue in cluelist on communication.ClueId equals clue.Id into clueGroup
                               from clue in clueGroup.DefaultIfEmpty()
                               join bus in businessopportunitylist on communication.BusinessOpportunityId equals bus.Id into busGroup
                               from bus in busGroup.DefaultIfEmpty()
                               join communicationtype in communicationtypelist on communication.ExpectedDateId equals communicationtype.Id into typeGroup
                               from communicationtype in typeGroup.DefaultIfEmpty()
                               join reply in replylist on communicationtype.CustomReplyId equals reply.Id into replyGroup
                               from reply in replyGroup.DefaultIfEmpty()
                               join user in userlist on cus.UserId equals user.Id into userGroup
                               from user in userGroup.DefaultIfEmpty()
                               select new ContactCommunicationDto
                               {
                                   Id =communication.Id,
                                   CustomerId=communication.CustomerId,
                                   CustomerName=cus.CustomerName,
                                   ClueId=communication.ClueId,
                                   ClueName=clue.ClueName,
                                   BusinessOpportunityId=communication.BusinessOpportunityId,
                                   BusinessOpportunityName=bus.BusinessOpportunityName,
                                   CreatorId=communication.CreatorId,
                                   CreationTime=communication.CreationTime,
                                   Content=communication.Content,
                                   AttachmentUrl=communication.AttachmentUrl,
                                   ExpectedDateId=communication.ExpectedDateId,
                                   CommunicationTypeName=communicationtype.CommunicationTypeName,
                                   NextContactTime=communication.NextContactTime,
                                   FollowUpStatus=communication.FollowUpStatus,
                                   Comments=communication.Comments,
                                   CustomReplyId=communicationtype.CustomReplyId,
                                   CustomReplyName=reply.CustomReplyName,
                                   IsServe=communication.IsServe,
                                   UserId=user.Id,
                                   UserName=user.UserName
                               };
                    // 联系对象类型筛选
                    switch (dto.ContactTargetType)
                    {
                        case ContactTargetType.Customer:
                            list = list.Where(x => x.CustomerId != null);
                            break;
                        case ContactTargetType.Clue:
                            list = list.Where(x => x.ClueId != null);
                            break;
                        case ContactTargetType.Business:
                            list = list.Where(x => x.BusinessOpportunityId != null);
                            break;
                        // 其他类型可按需补充
                        case ContactTargetType.All:
                        default:
                            // 不做过滤
                            break;
                    }

                    // 临时注释掉所有筛选
                    //var all = list.ToList(); // 断点：看all里有没有CustomerId不为null的数据

                    // type: 0=全部，1=我负责的，2=我创建的
                    // 查看范围过滤
                    if (dto.type == 1 && dto.AssignedTo.HasValue) // 我负责的
                    {
                        list = list.Where(x => x.UserId == dto.AssignedTo.Value);
                    }
                    else if (dto.type == 2 && dto.AssignedTo.HasValue) // 我创建的
                    {
                        list = list.Where(x => x.CreatorId == dto.AssignedTo.Value);
                    }

                    //查询条件
                    //根据联系内容模糊查询
                    if (!string.IsNullOrEmpty(dto.Keyword))
                    {
                        list = list.Where(x => x.Comments.Contains(dto.Keyword));
                    }
                    // 时间区间筛选（创建时间）
                    if (dto.StartTime.HasValue && dto.EndTime.HasValue)
                    {
                        list = list.Where(x => x.CreationTime >= dto.StartTime && x.CreationTime <= dto.EndTime);
                    }
                    else if (dto.StartTime.HasValue)
                    {
                        list = list.Where(x => x.CreationTime >= dto.StartTime);
                    }
                    else if (dto.EndTime.HasValue)
                    {
                        list = list.Where(x => x.CreationTime <= dto.EndTime);
                    }

                    // 排序
                    if (dto.OrderBy.HasValue)
                    {
                        list = (dto.OrderBy.Value, dto.OrderDesc) switch
                        {
                            (TimeField.CreateTime, true) => list.OrderByDescending(x => x.CreationTime),
                            (TimeField.CreateTime, false) => list.OrderBy(x => x.CreationTime),

                            (TimeField.NextContact, true) => list.OrderByDescending(x => x.NextContactTime),
                            (TimeField.NextContact, false) => list.OrderBy(x => x.NextContactTime),
                        };
                    }

                    //用ABP框架的分页
                    var res = list.PageResult(dto.PageIndex, dto.PageSize);
                    //构建分页结果对象
                    return new PageInfoCount<ContactCommunicationDto>
                    {
                        TotalCount = res.RowCount,
                        PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / dto.PageSize),
                        Data = res.Queryable.ToList()
                    };
                }, () => new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(5)     //设置缓存过期时间为5分钟
                });
                return ApiResult<PageInfoCount<ContactCommunicationDto>>.Success(ResultCode.Success, redislist);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 无查询条件、无分页，显示所有联系沟通列表（可根据线索id/客户id/商机id筛选）
        /// </summary>
        /// <param name="id">线索id/客户id/商机id</param>
        /// <param name="targetType">目标类型：1-线索，2-客户，3-商机</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<ContactCommunicationDto>>> GetAllContactCommunications(Guid id, int targetType)
        {
            try
            {
                var customerlist = await customerRepository.GetQueryableAsync();
                var cluelist = await cluerepository.GetQueryableAsync();
                var userlist = await userRepository.GetQueryableAsync();
                var replylist = await replyRepository.GetQueryableAsync();
                var businessopportunitylist = await businessopportunityrepository.GetQueryableAsync();
                var communicationtypelist = await communicationTypeRepository.GetQueryableAsync();
                var contactCommunicationlist = await contactCommunicationRepository.GetQueryableAsync();
                var list = from communication in contactCommunicationlist
                           join cus in customerlist on communication.CustomerId equals cus.Id into cusGroup
                           from cus in cusGroup.DefaultIfEmpty()
                           join clue in cluelist on communication.ClueId equals clue.Id into clueGroup
                           from clue in clueGroup.DefaultIfEmpty()
                           join bus in businessopportunitylist on communication.BusinessOpportunityId equals bus.Id into busGroup
                           from bus in busGroup.DefaultIfEmpty()
                           join communicationtype in communicationtypelist on communication.ExpectedDateId equals communicationtype.Id into typeGroup
                           from communicationtype in typeGroup.DefaultIfEmpty()
                           join reply in replylist on communicationtype.CustomReplyId equals reply.Id into replyGroup
                           from reply in replyGroup.DefaultIfEmpty()
                           join user in userlist on cus.UserId equals user.Id into userGroup
                           from user in userGroup.DefaultIfEmpty()
                           select new ContactCommunicationDto
                           {
                               Id = communication.Id,
                               CustomerId = communication.CustomerId,
                               CustomerName = cus != null ? cus.CustomerName : null,
                               ClueId = communication.ClueId,
                               ClueName = clue != null ? clue.ClueName : null,
                               BusinessOpportunityId = communication.BusinessOpportunityId,
                               BusinessOpportunityName = bus != null ? bus.BusinessOpportunityName : null,
                               CreatorId = communication.CreatorId,
                               CreationTime = communication.CreationTime,
                               Content = communication.Content,
                               AttachmentUrl = communication.AttachmentUrl,
                               ExpectedDateId = communication.ExpectedDateId,
                               CommunicationTypeName = communicationtype != null ? communicationtype.CommunicationTypeName : null,
                               NextContactTime = communication.NextContactTime,
                               FollowUpStatus = communication.FollowUpStatus,
                               Comments = communication.Comments,
                               CustomReplyId = communicationtype != null ? communicationtype.CustomReplyId : Guid.Empty,
                               CustomReplyName = reply != null ? reply.CustomReplyName : null,
                               IsServe = communication.IsServe,
                               UserId = cus != null ? cus.UserId : Guid.Empty,
                               UserName = user != null ? user.UserName : null,
                           };

                // 根据目标类型筛选数据
                switch (targetType)
                {
                    case 1: // 线索
                        list = list.Where(x => x.ClueId == id);
                        break;
                    case 2: // 客户
                        list = list.Where(x => x.CustomerId == id);
                        break;
                    case 3: // 商机
                        list = list.Where(x => x.BusinessOpportunityId == id);
                        break;
                    default:
                        break;
                }
                
                var result = list.ToList();
                return ApiResult<List<ContactCommunicationDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception ex)
            {
                logger.LogError("获取所有联系沟通信息出错！" + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 获取联系沟通详情信息
        /// </summary>
        /// <param name="id">要查询的联系沟通ID</param>
        /// <returns></returns>
        /// 
        [HttpGet]

        public async Task<ApiResult<ContactCommunicationDto>> GetcontactCommunicationById(Guid id)
        {
            try
            {
                var contactCommunication = await contactCommunicationRepository.GetAsync(x => x.Id == id);
                if (contactCommunication == null)
                {
                    return ApiResult<ContactCommunicationDto>.Fail("联系沟通信息不存在", ResultCode.NotFound);
                }
                return ApiResult<ContactCommunicationDto>.Success(ResultCode.Success, ObjectMapper.Map<ContactCommunication, ContactCommunicationDto>(contactCommunication));

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 删除联系沟通信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult<ContactCommunicationDto>> DelcontactCommunication(Guid id)
        {
            try
            {
                var contactCommunication = await contactCommunicationRepository.GetAsync(x => x.Id == id);
                if (contactCommunication == null)
                {
                    return ApiResult<ContactCommunicationDto>.Fail("联系沟通信息不存在", ResultCode.NotFound);
                }
                contactCommunication.IsDeleted = true;
                await contactCommunicationRepository.UpdateAsync(contactCommunication);
                //清除缓存，确保数据一致性
                await ClearAbpCacheAsync();
                return ApiResult<ContactCommunicationDto>.Success(ResultCode.Success, ObjectMapper.Map<ContactCommunication, ContactCommunicationDto>(contactCommunication));
            }
            catch (Exception ex)
            {
                logger.LogError("删除联系沟通信息出错！" + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 修改联系沟通信息
        /// </summary>
        /// <param name="id">要修改的联系沟通ID</param>
        /// <param name="dto">联系沟通信息</param>
        /// <returns></returns>
        /// 
        [HttpPut]
        public async Task<ApiResult<CreateUpdateContactCommunicationDto>> UpdcontactCommunication(Guid id, CreateUpdateContactCommunicationDto dto)
        {
            try
            {
                var contactCommunication = await contactCommunicationRepository.GetAsync(x => x.Id == id);
                if (contactCommunication == null)
                {
                    return ApiResult<CreateUpdateContactCommunicationDto>.Fail("联系沟通信息不存在", ResultCode.NotFound);
                }
                var contactCommunicationDto = ObjectMapper.Map(dto, contactCommunication);
                await contactCommunicationRepository.UpdateAsync(contactCommunicationDto);
                //清除缓存，确保数据一致性
                await ClearAbpCacheAsync();
                return ApiResult<CreateUpdateContactCommunicationDto>.Success(ResultCode.Success, ObjectMapper.Map<ContactCommunication, CreateUpdateContactCommunicationDto>(contactCommunicationDto));

            }
            catch (Exception ex)
            {
                logger.LogError("修改联系沟通信息出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取沟通类型下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<CommunicationTypeDto>>> GetCommunicationTypeSelectList()
        {
            try
            {

                var CommunicationTypeList = await communicationTypeRepository.GetQueryableAsync();
                var result = CommunicationTypeList
                    .Select(u => new
                    {
                        IdString = u.Id.ToString(),
                        u.CommunicationTypeName
                    })
                    .AsEnumerable()
                    .Where(u => Guid.TryParse(u.IdString, out _))
                    .Select(u => new CommunicationTypeDto
                    {
                        Id = Guid.Parse(u.IdString),
                        CommunicationTypeName = u.CommunicationTypeName,
                    })
                    .ToList();
                return ApiResult<List<CommunicationTypeDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception ex)
            {
                logger.LogError("获取沟通类型下拉框数据出错！" + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 根据沟通类型id获取对应的自定义回复内容
        /// </summary>
        /// <param name="typeId">沟通类型id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<CustomReplyDto>>> GetCustomReplyByType(Guid typeId)
        {
            try
            {
                // 1. 查找沟通类型
                var communicationTypeList = await communicationTypeRepository.GetQueryableAsync();
                var commType = communicationTypeList.FirstOrDefault(x => x.Id == typeId);
                if (commType == null)
                {
                    return ApiResult<List<CustomReplyDto>>.Fail("未找到对应的沟通类型", ResultCode.NotFound);
                }
                var customReplyId = commType.CustomReplyId;

                // 2. 查找自定义回复
                var customReplyList = await replyRepository.GetQueryableAsync();
                var customReply = customReplyList.FirstOrDefault(x => x.Id == customReplyId);
                if (customReply == null)
                {
                    return ApiResult<List<CustomReplyDto>>.Fail("未找到对应的自定义回复", ResultCode.NotFound);
                }

                var result = new CustomReplyDto
                {
                    Id = customReply.Id,
                    CustomReplyName = customReply.CustomReplyName,
                    CustomReplyEnglishName = customReply.CustomReplyEnglishName,
                    CreateTime = customReply.CreateTime
                };
                return ApiResult<List<CustomReplyDto>>.Success(ResultCode.Success, new List<CustomReplyDto> { result });
            }
            catch (Exception ex)
            {
                logger.LogError($"获取typeId={typeId}的自定义回复出错！" + ex.Message);
                throw;
            }
        }
    }
}
