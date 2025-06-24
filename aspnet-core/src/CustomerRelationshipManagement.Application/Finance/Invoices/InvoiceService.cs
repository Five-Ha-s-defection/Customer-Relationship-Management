using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.FinanceInfo.Invoices;
using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.Finance.Invoices
{
    public class InvoiceService : ApplicationException, IInvoiceService
    {
        private readonly IRepository<Invoice, Guid> repository;

        public InvoiceService(IRepository<Invoice,Guid> repository)
        {
            this.repository = repository;
        }

        public Task<ApiResult<InvoiceDTO>> InvoiceAsync(CreateUpdateInvoiceDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<InvoiceDTO>> DeleteInvoiceAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<InvoiceDTO>> GetInvoiceByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<PageInfoCount<InvoiceDTO>>> GetInvoiceListAsync(InvoiceDTO input)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<InvoiceDTO>> UpdateInvoiceAsync(Guid id, CreateUpdateInvoiceDto input)
        {
            throw new NotImplementedException();
        }
    }
}
