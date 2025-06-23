using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CustomerRelationshipManagement.Application.Contracts.DTOS.File
{
    public class FileUploadInputDto
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string Name { get; set; }
    }
} 