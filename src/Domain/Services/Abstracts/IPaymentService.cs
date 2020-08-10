using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services
{
    using Entities;
    using Models;

    public interface IPaymentService
    {
        Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request, Merchant merchant);
        Task<IEnumerable<GetPaymentsResponse>> GetAll(Merchant merchant);
    }
}
