using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBAC.UserInfos
{
    public interface IJwtHelper
    {
        string GenerateToken(Guid userId,string userName);
    }
}
