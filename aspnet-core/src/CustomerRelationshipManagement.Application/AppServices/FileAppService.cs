using CustomerRelationshipManagement.Application.Contracts.DTOS.File;
using CustomerRelationshipManagement.Application.Contracts.IAppServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Application.AppServices
{
    public class FileAppService : ApplicationService, IFileAppService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileAppService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<FileUploadOutputDto> UploadAsync(FileUploadInputDto input)
        {
            var file = input.File;
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }

            var uploadsFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var fileName = $"{Guid.NewGuid()}_{input.Name}";
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"/uploads/{fileName}";

            return new FileUploadOutputDto
            {
                FileName = fileName,
                Url = url
            };
        }
    }
} 