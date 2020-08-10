using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories
{
    using Data;
    using Entities;
    using System;
    using System.Collections.Generic;

    public interface IPaymentRepository : IRepositoryBase<Payment>
    {
        Task<IEnumerable<Payment>> GetAllByMerchant(Merchant merchant);
    }
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(Database context) : base(context) { }

        public async Task<IEnumerable<Payment>> GetAllByMerchant(Merchant merchant)
        {
            if (merchant == null)
                throw new ArgumentNullException(nameof(merchant));

            return await db.Payments.Where(x => x.MerchantId == merchant.Id).OrderBy(x=> x.CreatedOn).ToListAsync();
        }
    }
}
