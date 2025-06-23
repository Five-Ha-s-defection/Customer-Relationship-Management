using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Paging;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.Payments
{
    public interface IPaymentService
    {
        Task<ApiResult<PaymentDTO>> InsertPayment(CreateUpdatePaymentDTO createUpdatePaymentDTO);

        //Task<ApiResult<PageInfoCount<PaymentDTO>>> GetPayment(PaymentSearchDTO paymentSearchDTO);
    }
}
