using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    using Entities;

    public interface IPaymentRepository : IRepositoryBase<Payment>
    {
        Task<IEnumerable<Payment>> GetAllByMerchant(Merchant merchant);
    }
}
