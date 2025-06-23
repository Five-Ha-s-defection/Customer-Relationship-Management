using CustomerRelationshipManagement.ApiResult;
using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Finance
{
    /// <summary>
    /// 应收款服务接口
    /// 定义应收款相关的业务操作契约
    /// </summary>
    public interface IReceivablesService:IApplicationService
    {
        /// <summary>
        /// 添加应收款信息
        /// </summary>
        /// <param name="createUpdateReceibablesDto">应收款创建/更新数据传输对象</param>
        /// <returns>操作结果，包含创建的应收款信息</returns>
        Task<ApiResult<ReceivablesDTO>> InsertAsync(CreateUpdateReceibablesDto createUpdateReceibablesDto);

        /// <summary>
        /// 获取应收款分页列表
        /// 支持多种查询条件过滤，并使用Redis缓存提高查询性能
        /// </summary>
        /// <param name="receivablesSearchDto">应收款搜索条件</param>
        /// <returns>分页查询结果</returns>
        Task<ApiResult<PageInfoCount<ReceivablesDTO>>> GetPageAsync(ReceivablesSearchDto receivablesSearchDto);

        /// <summary>
        /// 根据ID获取应收款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<ReceivablesDTO>> GetByIdAsync(Guid id);

        /// <summary>
        /// 修改应收款信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="createUpdateReceibablesDto"></param>
        /// <returns></returns>
        Task<ApiResult<ReceivablesDTO>> UpdateAsync(Guid id, CreateUpdateReceibablesDto createUpdateReceibablesDto);

        /// <summary>
        /// 删除应收款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<ReceivablesDTO>> DeleteAsync(Guid id);
    }
}
