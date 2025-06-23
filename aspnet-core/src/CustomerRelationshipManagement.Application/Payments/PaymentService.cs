using CustomerRelationshipManagement.ApiResults;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.Payments
{
    public class PaymentService:ApplicationService, IPaymentService
    {
        private readonly IRepository<Payment, Guid> repository;

        public PaymentService(IRepository<Payment, Guid> repository)
        {
            this.repository = repository;
        }
        public async Task<ApiResult<PaymentDTO>> InsertPayment(CreateUpdatePaymentDTO createUpdatePaymentDTO)
        {
            var pay = ObjectMapper.Map<CreateUpdatePaymentDTO, Payment>(createUpdatePaymentDTO);
            if(string.IsNullOrEmpty(pay.PaymentCode))
            {
                Random random = new Random();
                pay.PaymentCode = $"R{DateTime.Now.ToString("yyyyMMdd")}-{random.Next(1000, 10000)}";
            }
            else
            {
                pay.PaymentCode = $"R{pay.PaymentCode}";
            }
                var payment = await repository.InsertAsync(pay);
            return ApiResult<PaymentDTO>.Success(ResultCode.Success, ObjectMapper.Map<Payment, PaymentDTO>(payment));
        }


    }
}
