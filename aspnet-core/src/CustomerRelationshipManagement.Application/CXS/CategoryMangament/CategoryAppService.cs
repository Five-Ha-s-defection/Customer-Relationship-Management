using AutoMapper.Internal.Mappers;
using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Categorys;
using CustomerRelationshipManagement.CXS.ProductManagement;
using CustomerRelationshipManagement.DTOS.CategoryMangamentDto;
using CustomerRelationshipManagement.Interfaces.ICategoryAppService;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.CXS.DTOS.CategoryMangamentDto;
using CustomerRelationshipManagement.CXS.ErrorCode;
using CustomerRelationshipManagement.ProductCategory.Categorys;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;


namespace CustomerRelationshipManagement.CXS.CategoryMangament
{
    /// <summary>
    /// 产品类型
    /// </summary>
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

        public async Task<ApiResult> DeletedCategory(Guid id)
        {
            var category = await categoryRepository.GetAsync(x => x.Id == id);
            if (category == null)
            {
                return ApiResult.Fail("产品类型不存在", ResultCode.Fail);
            }
            await categoryRepository.DeleteAsync(category);
            return ApiResult.Success(ResultCode.Success);
        }
        /// <summary>
        /// 获取产品类型列表
        /// </summary>
        /// <param name="apiPaging"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<ApiResult<IList<CategoryDtos>>> GetCategory([FromQuery] CategoryDtos CategoryDtos)
        {
            try
            {
                //对产品类型进行预查询
                var categorylist = await categoryRepository.GetQueryableAsync();
                //查询条件

                //分页
                var categorypaging = categorylist.OrderByDescending(x => x.Id).Skip(CategoryDtos.PageIndex).Take(CategoryDtos.PageSize);


                //将数据通过映射转换
                var categorydtos=ObjectMapper.Map<IList<Category>, IList<CategoryDtos>>(categorypaging.ToList());
                //返回apiresult
                return ApiResult<IList<CategoryDtos>>.Success(ResultCode.Success, categorydtos);
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
                return ApiResult<CreateUpdateCategoryDtos>.Success(ResultCode.Success,ObjectMapper.Map<Category, CreateUpdateCategoryDtos>(categoryDto));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
