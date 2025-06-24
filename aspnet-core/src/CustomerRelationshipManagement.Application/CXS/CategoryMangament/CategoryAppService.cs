using AutoMapper.Internal.Mappers;
using CustomerRelationshipManagement.CXS.DTOS.CategoryMangamentDto;
using CustomerRelationshipManagement.CXS.ErrorCode;
using CustomerRelationshipManagement.CXS.ProductManagement;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace CustomerRelationshipManagement.CXS.CategoryMangament
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class CategoryAppService : ApplicationService, ICategoryAppService
    {
        private readonly IRepository<Category> categoryRepository;
        public CategoryAppService(IRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        /// <summary>
        ///  新增产品类型
        /// </summary>
        /// <param name="createCategory"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CategoryDtos> AddCategory(CreateUpdateCategoryDtos createCategory)
        {
            var categoryAdd = ObjectMapper.Map<CreateUpdateCategoryDtos, Category>(createCategory);
            //保存到数据库
            categoryAdd = await categoryRepository.InsertAsync(categoryAdd);
            //将数据库操作成功后的 Category 实体转换为 CategoryDtos 对象
            return ObjectMapper.Map<Category, CategoryDtos>(categoryAdd);
        }
        /// <summary>
        /// // 删除产品类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpDelete]

        public async Task<ApiResult<CategoryDtos>> DeletedCategory(Guid id)
        {
            var category = await categoryRepository.GetAsync(x => x.Id == id);
            if (category == null)
            {
                return ApiResult<CategoryDtos>.Fail("产品类型不存在", ResultCode.Fail);
            }
            await categoryRepository.DeleteAsync(category);
            return ApiResult<CategoryDtos>.Success(ObjectMapper.Map<Category, CategoryDtos>(category), ResultCode.Success);
        }
        /// <summary>
        /// 获取产品类型列表
        /// </summary>
        /// <param name="apiPaging"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<ApiResult<ApiPaging<CategoryDtos>>> GetCategory([FromQuery] ApiPaging apiPaging)
        {
            try
            {
                //对产品类型进行预查询
                var categorylist = await categoryRepository.GetQueryableAsync();
                //查询条件
                //分页
                var totalCount = categorylist.Count();
                var totalPage = (int)Math.Ceiling((double)categorylist.Count() / apiPaging.PageSize);
                var pageCatesgory = categorylist.OrderByDescending(x => x.Id).Skip((apiPaging.PageIndex - 1) * apiPaging.PageSize).Take(apiPaging.PageSize).ToList();
                return ApiResult<ApiPaging<CategoryDtos>>.Success(new ApiPaging<CategoryDtos>
                {
                    Data = ObjectMapper.Map<List<Category>, List<CategoryDtos>>(pageCatesgory),
                    TotalCount = totalCount,
                    TotalPage = totalPage,
                }, ResultCode.Success);
                //将数据通过映射转换

                //返回apiresult

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 获取产品类型详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]

        public async Task<ApiResult<CategoryDtos>> GetCategoryId(Guid id)
        {
            try
            {
                var category = await categoryRepository.GetAsync(x => x.Id == id);
                if (category == null)
                {
                    return ApiResult<CategoryDtos>.Fail("产品类型不存在", ResultCode.Fail);
                }
                return ApiResult<CategoryDtos>.Success(ObjectMapper.Map<Category, CategoryDtos>(category), ResultCode.Success);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 修改产品类型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateCategory"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 
        [HttpPut]
        public async Task<ApiResult<CreateUpdateCategoryDtos>> UdpateCategory(Guid id, CreateUpdateCategoryDtos updateCategory)
        {
            try
            {
                var category = await categoryRepository.GetAsync(x=>x.Id==id);
                if (category == null)
                {
                    return ApiResult<CreateUpdateCategoryDtos>.Fail("未找到该数据", ResultCode.NotFound);
                }
                var categoryDto=ObjectMapper.Map(updateCategory, category);
                await categoryRepository.UpdateAsync(category);
                return ApiResult<CreateUpdateCategoryDtos>.Success(ObjectMapper.Map<Category, CreateUpdateCategoryDtos>(categoryDto), ResultCode.Success);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
