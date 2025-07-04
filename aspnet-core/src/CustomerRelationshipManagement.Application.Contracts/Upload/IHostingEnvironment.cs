using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.Upload
{
    public interface IHostingEnvironment
    {
        string WebRootPath { get; }
    }
}
