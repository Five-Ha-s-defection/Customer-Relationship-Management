using CustomerRelationshipManagement.ApiResults;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.Payments
{
    public interface IPaymentService
    {
        Task<ApiResult<PaymentDTO>> InsertPayment(CreateUpdatePaymentDTO createUpdatePaymentDTO);
    }
}
