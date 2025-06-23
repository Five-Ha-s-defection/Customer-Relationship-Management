using CustomerRelationshipManagement.Application.Contracts.DTOS.File;
using CustomerRelationshipManagement.Application.Contracts.IAppServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace CustomerRelationshipManagement.HttpApi.Host.Controllers
{
    [Route("api/files")]
    public class FilesController : AbpController
    {
        private readonly IFileAppService _fileAppService;

        public FilesController(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [HttpPost("upload")]
        public async Task<FileUploadOutputDto> UploadAsync([FromForm] FileUploadInputDto input)
        {
            return await _fileAppService.UploadAsync(input);
        }
    }
} 