using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.DTOS.CategoryMangamentDto
{
    public class CategoryDtosList:EntityDto<Guid>
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
    }
}
