using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories
{
    using Data;
    using Entities;

    public interface IPaymentRepository : IRepositoryBase<Payment>
    {

    }
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(Database context) : base(context) { }
    }
}
