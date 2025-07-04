using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.Finance.Incoices;
using CustomerRelationshipManagement.Finance.Invoices;
using CustomerRelationshipManagement.Paging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.Interfaces.IFinance.Invoices
{
    public interface IInvoiceService
    {
        Task<ApiResult<InvoiceDTO>> InvoiceAsync(CreateUpdateInvoiceDto createUpdateInvoiceDto);
        Task<ApiResult<InvoiceDTO>> GetInvoiceByIdAsync(Guid id);
        Task<ApiResult<PageInfoCount<InvoiceDTO>>> GetInvoiceListAsync(InvoiceSearchDto invoiceSearchDto);
        Task<ApiResult<InvoiceDTO>> UpdateInvoiceAsync(Guid id,CreateUpdateInvoiceDto createUpdateInvoiceDto);
        Task<ApiResult<InvoiceDTO>> DeleteInvoiceAsync(Guid id);

        Task<ApiResult<InvoiceDTO>> DeleteAsync([FromBody] Guid[] ids);
    }
}
