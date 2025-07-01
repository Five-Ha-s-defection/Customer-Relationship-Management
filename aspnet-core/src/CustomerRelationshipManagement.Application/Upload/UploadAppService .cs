using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class UploadAppService:ApplicationService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor httpContextAccessor;

        // 可配置
        private readonly string[] AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx", ".xlsx", ".zip" };
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB

        public UploadAppService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("upload-file")]
        public async Task<string> UploadFileAsync(IFormFile file)
        {

            var request = httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                throw new UserFriendlyException("无法获取请求上下文");
            }
            if (file == null || file.Length == 0)
            {
                throw new UserFriendlyException("文件不能为空");
            }

            if (file.Length > MaxFileSize)
            {
                throw new UserFriendlyException("文件大小不能超过 10MB");
            }

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (Array.IndexOf(AllowedExtensions, ext) == -1)
            {
                throw new UserFriendlyException($"不支持的文件类型: {ext}");
            }

            // 构建文件保存路径：/wwwroot/upload/file/yyyy/MM/
            var dateFolder = DateTime.Now.ToString("yyyy/MM");
            var folderPath = Path.Combine(_env.WebRootPath, "upload", "file", dateFolder);
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
