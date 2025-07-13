using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.Finance.Incoices;
using CustomerRelationshipManagement.DTOS.Finance.Payments;
using CustomerRelationshipManagement.Paging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Content;

namespace CustomerRelationshipManagement.Interfaces.IFinance.Invoices
{
    public interface IInvoiceService
    {
        Task<ApiResult<InvoiceDTO>> InvoiceAsync(CreateUpdateInvoiceDto createUpdateInvoiceDto);
        Task<ApiResult<InvoiceDTO>> GetInvoiceByIdAsync(Guid id);
        Task<ApiResult<PageInfoCount<InvoiceDTO>>> GetInvoiceListAsync(InvoiceSearchDto invoiceSearchDto);
        Task<IRemoteStreamContent> GetExportAsyncExcel();
        Task<ApiResult<InvoiceDTO>> UpdateInvoiceAsync(Guid id,CreateUpdateInvoiceDto createUpdateInvoiceDto);
        Task<ApiResult<List<PaymentInvoiceDto>>> GetLogs(Guid? PaymentId);
        Task<ApiResult<InvoiceDTO>> DeleteInvoiceAsync(Guid id);

        Task<ApiResult<InvoiceDTO>> DeleteAsync([FromBody] Guid[] ids);
    }
}
