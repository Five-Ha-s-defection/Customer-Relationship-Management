using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.CXS.ProductManagementDto
{
    /// <summary>
    /// 文件传输对象，用于返回文件流给前端
    /// </summary>
    public class FileDto
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件字节流
        /// </summary>
        public byte[] FileBytes { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType { get; set; }

        public FileDto(string fileName, byte[] fileBytes, string contentType)
        {
            FileName = fileName;
            FileBytes = fileBytes;
            ContentType = contentType;
        }
    }
}
