using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.PaymentMethods
{
    public class PaymentMethod:Entity<Guid>
    {
        public string PaymentMethodName { get; set; }
    }
}
