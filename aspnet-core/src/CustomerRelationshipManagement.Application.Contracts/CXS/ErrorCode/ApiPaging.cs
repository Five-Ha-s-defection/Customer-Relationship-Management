using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.CXS.ErrorCode
{
    public class ApiPaging
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? Keyword { get; set; }
    }

    public class ApiPaging<T>
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public List<T> Data { get; set; } = new List<T>();
    }
}
