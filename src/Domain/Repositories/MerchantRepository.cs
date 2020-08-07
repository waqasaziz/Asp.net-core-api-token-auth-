using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories
{
    using Data;
    using Entities;

    public interface IMerchantRepository : IRepositoryBase<Merchant>
    {
        Task<Merchant> FindByUserName(string userName);
        Task<Merchant> FindByToken(string token);
    }
    public class MerchantRepository : RepositoryBase<Merchant>, IMerchantRepository
    {
        public MerchantRepository(Database context) : base(context) { }

        public Task<Merchant> FindByUserName(string userName) =>
            db.Merchants.SingleOrDefaultAsync(x => x.Username == userName);

        public Task<Merchant> FindByToken(string token) =>
           db.Merchants.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
    }
}
