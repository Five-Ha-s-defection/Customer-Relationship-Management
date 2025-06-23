using CustomerRelationshipManagement.Application.Contracts.DTOS.File;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Application.Contracts.IAppServices
{
    public interface IFileAppService : IApplicationService
    {
        Task<FileUploadOutputDto> UploadAsync(FileUploadInputDto input);
    }
} 