using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.Finance.Payments;
using CustomerRelationshipManagement.Paging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Content;

namespace CustomerRelationshipManagement.Interfaces.IFinance.Payments
{
    public interface IPaymentService
    {
        Task<ApiResult<PaymentDTO>> InsertPayment(CreateUpdatePaymentDTO createUpdatePaymentDTO);

        Task<ApiResult> Approve(Guid paymentId, Guid approverId, bool isPass, string? comment);

        Task<ApiResult<PageInfoCount<PaymentDTO>>> GetPayment(PaymentSearchDTO paymentSearchDTO);

        Task<IRemoteStreamContent> GetExportAsyncExcel();

        Task<ApiResult<PaymentDTO>> UpdatePayment(Guid id, CreateUpdatePaymentDTO createUpdatePaymentDTO);

        Task<ApiResult<PaymentDTO>> DeleteAsync(Guid id);

        Task<ApiResult<PaymentDTO>> DeleteAllAsync([FromBody] Guid[] ids);

        Task<ApiResult<PaymentDTO>> GetByIdAsync(Guid id);
    }
}
