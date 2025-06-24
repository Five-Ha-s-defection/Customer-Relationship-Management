using CustomerRelationshipManagement.ApiResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Paging;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.FinanceInfo.Payments
{
    public interface IPaymentService
    {
        Task<ApiResult<PaymentDTO>> InsertPayment(CreateUpdatePaymentDTO createUpdatePaymentDTO);

        Task<ApiResult<PageInfoCount<PaymentDTO>>> GetPayment(PaymentSearchDTO paymentSearchDTO);

        Task<ApiResult<PaymentDTO>> UpdatePayment(Guid id, CreateUpdatePaymentDTO createUpdatePaymentDTO);

        Task<ApiResult<PaymentDTO>> DeleteAsync(Guid id);

        Task<ApiResult<PaymentDTO>> GetByIdAsync(Guid id);
    }
}
