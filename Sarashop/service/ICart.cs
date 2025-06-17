using Sarashop.Models;
using Sarashop.service.IServices;

namespace Sarashop.service
{
    public interface ICart : IService<Cart>
    {
        Task<Cart> AddToCart(string userId, int productid, CancellationToken cancellationToken);
        Task<IEnumerable<Cart>> ShowUserCart(string userId);
        Task<bool> RemoveRangeAsync(List<Cart> items, CancellationToken cancellationToken = default);


    }
}
