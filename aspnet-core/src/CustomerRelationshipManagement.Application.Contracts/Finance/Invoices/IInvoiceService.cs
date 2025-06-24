using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.FinanceInfo.Invoices;
using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.Finance.Invoices
{
    public interface IInvoiceService
    {
        Task<ApiResult<InvoiceDTO>> InvoiceAsync(CreateUpdateInvoiceDto input);
        Task<ApiResult<InvoiceDTO>> GetInvoiceByIdAsync(Guid id);
        Task<ApiResult<PageInfoCount<InvoiceDTO>>> GetInvoiceListAsync(InvoiceDTO input);
        Task<ApiResult<InvoiceDTO>> UpdateInvoiceAsync(Guid id,CreateUpdateInvoiceDto input);
        Task<ApiResult<InvoiceDTO>> DeleteInvoiceAsync(Guid id);
    }
}
