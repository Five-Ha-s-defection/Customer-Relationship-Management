using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.CustomerProcess.Customers.Helpers
{
    /// <summary>
    /// 上传图片（附文本）返回值
    /// </summary>
    public class EditorUploadResult
    {
        public int errno { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
