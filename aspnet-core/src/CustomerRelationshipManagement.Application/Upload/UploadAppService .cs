using Azure.Core;
using CustomerRelationshipManagement.DTOS.UploadDto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Upload
{
    /// <summary>
    /// 文件上传
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/app/common")]
    public class UploadAppService : ApplicationService
    {
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IOptions<FileUploadOptions> fileUploadOptions;


        // 可配置
        private readonly string[] AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx", ".xlsx", ".zip" };
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB

        public UploadAppService(IHostingEnvironment env, IHttpContextAccessor httpContextAccessor,IOptions<FileUploadOptions> fileUploadOptions)
        {
            _env = env;
            this.httpContextAccessor = httpContextAccessor;
            this.fileUploadOptions = fileUploadOptions;
        }

        [HttpPost("upload-file")]
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            // 获取 HttpRequest 对象
            var request = httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                throw new UserFriendlyException("无法获取请求上下文");
            }
            // 校验是否为空
            if (file == null || file.Length == 0)
            {
                throw new UserFriendlyException("文件不能为空");
            }
            // 校验文件大小（配置的是 MB，要转成字节）
            if (file.Length > fileUploadOptions.Value.MaxFileSize * 1024 * 1024)
            {
                throw new UserFriendlyException($"文件大小不能超过 {fileUploadOptions.Value.MaxFileSize}MB");
            }
            // 校验扩展名
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            // 判断扩展名是否在允许的范围内
            if (!fileUploadOptions.Value.AllowedExtensions.Contains(ext))
            {
                throw new UserFriendlyException($"不支持的文件类型: {ext}");
            }

            // 构建文件保存路径：/wwwroot/upload/file/yyyy/MM/
            var dateFolder = DateTime.Now.ToString("yyyy/MM/dd");
            var folderPath = Path.Combine(_env.WebRootPath, "upload", "file", dateFolder);
            //创建文件夹
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // 构建唯一文件名
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(folderPath, fileName);

            // 保存文件
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 构建访问 URL
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var fileUrl = $"{baseUrl}/upload/file/{dateFolder}/{fileName}";

            return fileUrl;
        }
    }
}
