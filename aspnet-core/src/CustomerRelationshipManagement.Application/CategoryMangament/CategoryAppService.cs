using CustomerRelationshipManagement.DTOS.CategoryMangamentDto;
using CustomerRelationshipManagement.ErrorCode;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.CategoryMangament
{
    public class CategoryAppService : ApplicationException, ICategoryAppService
    {
        public Task<CategoryDtos> AddCategory(CreateUpdateCategoryDtos createCategory)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<CategoryDtos>> DeletedCategory(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<ApiPaging<CategoryDtos>>> GetCategory([FromQuery] ApiPaging apiPaging)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<CategoryDtos>> GetCategoryId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<CreateUpdateCategoryDtos>> UdpateCategory(Guid id, CreateUpdateCategoryDtos updateCategory)
        {
            throw new NotImplementedException();
        }
    }
}
