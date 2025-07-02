using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Json.SystemTextJson.JsonConverters;

namespace CustomerRelationshipManagement.Dtos.CrmContractDtos
{
    public class PageCrmContractDto: PagingInfo
    {
        /// <summary>
        /// 时间查询类型 0:创建时间 1:签订日期 2:生效日期 3:截至日期
        /// </summary>
        public int? SearchTimeType {  get; set; } = 0;

        /// <summary>
        /// 开始时间
        /// </summary>
        public string? BeginTime { get; set; } = string.Empty;

        /// <summary>
        /// 结束时间
        /// </summary>
        public string? EndTime { get; set; } = string.Empty;

        /// <summary>
        /// 查询的满足条件 0:不使用高级搜索 1:使用全部满足条件搜索 2:部分满足条件搜索
        /// </summary>
        public int? CheckType {  get; set; } = 0;

        /// <summary>
        /// 负责人ids
        /// </summary>
        public IList<Guid>? UserIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 创建人ids
        /// </summary>
        public IList<Guid>? CreateUserIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 所属客户id
        /// </summary>
        public Guid? CustomerId {  get; set; } = Guid.Empty;

        /// <summary>
        /// 合同名称
        /// </summary>
        public string? ContractName { get; set; } = string.Empty;

        /// <summary>
        /// 签订日期
        /// </summary>
        public string? SignDate {  get; set; } = string.Empty;

        /// <summary>
        /// 生效日期
        /// </summary>
        public string? CommencementDate { get; set; } = string.Empty;

        /// <summary>
        /// 截止日期
        /// </summary>
        public string? ExpirationDate { get; set; } = string.Empty;

        /// <summary>
        /// 经销商
        /// </summary>
        public string? Dealer { get; set; } = string.Empty;

        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal? ContractProceeds {  get; set; } = 0;
    }
}
