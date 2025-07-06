using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Industrys;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Sources;
using CustomerRelationshipManagement.Paging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;


namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.IClues
{
    public interface IClueService:IApplicationService
    {
        /// <summary>
        /// 清楚关于c:PageInfo,k的所有信息
        /// </summary>
        /// <returns></returns>
        Task ClearAbpCacheAsync();

        /// <summary>
        /// 添加线索信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<ClueDto>> AddClue(CreateUpdateClueDto dto);
        
        /// <summary>
        /// 显示线索信息
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        Task<ApiResult<PageInfoCount<ClueDto>>> ShowClue([FromQuery] SearchClueDto dto);

        /// <summary>
        /// 获取线索表详情
        /// </summary>
        /// <param name="id">要查看的线索ID</param>
        /// <returns></returns>
        Task<ApiResult<ClueDto>> GetClueById(Guid id);

        /// <summary>
        /// 删除线索信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<ClueDto>> DelClue(Guid id);

        /// <summary>
        /// 修改线索信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<CreateUpdateClueDto>> UpdClue(Guid id, CreateUpdateClueDto dto);

        /// <summary>
        /// 获取来源下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<SourceDto>>> GetSourceSelectList();

        /// <summary>
        /// 获取行业下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<IndustryDto>>> GetIndustrySelectList();
    }
}
