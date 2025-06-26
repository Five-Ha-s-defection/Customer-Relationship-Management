using System.Threading.Tasks;
using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBACDtos.Menus;

namespace CustomerRelationshipManagement.Application.Contracts.RBAC.Menus
{
    public interface IMenuAppService
    {
        Task<ApiResult> CreateMenuAsync(CreateOrUpdateMenuDto input);
        Task<ApiResult> UpdateMenuAsync(CreateOrUpdateMenuDto input);
        Task<ApiResult> DeleteMenuAsync(long id);
    }
} 