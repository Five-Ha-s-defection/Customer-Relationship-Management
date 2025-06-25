using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

<<<<<<<< HEAD:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/CustomerManegement/Cars/CarFrameNumber.cs
namespace CustomerRelationshipManagement.CustomerProcess.CustomerManegement.Cars
========
namespace CustomerRelationshipManagement.CustomerProcess.Cars
>>>>>>>> DevBranth:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/Cars/CarFrameNumber.cs
{
    /// <summary>
    /// 车架号表
    /// </summary>
    public class CarFrameNumber:Entity<Guid>
    {
        /// <summary>
        /// 车架号名称
        /// </summary>
        public string CarFrameNumberName { get; set; }
    }
}
