using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.CXS.DTOS.Base64UploadDtos
{
    /// <summary>
    /// Base64图片上传DTO
    /// </summary>
    public class Base64UploadDto
    {
        /// <summary>
        /// 文件名（带扩展名）
        /// </summary>
        public string FileName { get; set; } = string.Empty;
        /// <summary>
        /// Base64字符串（不带前缀）
        /// </summary>
        public string Base64 { get; set; }= string.Empty;
    }
}
