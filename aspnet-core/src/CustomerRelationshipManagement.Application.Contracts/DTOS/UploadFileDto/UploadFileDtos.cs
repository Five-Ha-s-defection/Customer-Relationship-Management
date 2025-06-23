using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.UploadFileDto
{
    public class UploadFileDtos
    {
        public IFormFile File { get; set; }
    }
}
