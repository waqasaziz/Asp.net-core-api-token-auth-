using System.Threading.Tasks;

namespace Domain.Repositories
{
    using Entities;
    public interface IMerchantRepository : IRepositoryBase<Merchant>
    {
        Task<Merchant> FindByUserName(string userName);
        Task<Merchant> FindByToken(string token);
    }
}
