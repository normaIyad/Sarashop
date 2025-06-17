using Sarashop.Models;
using Sarashop.service.IServices;

namespace Sarashop.service
{
    public interface IOrderItem : IService<OrderItem>
    {
        Task<List<OrderItem>> AddMnayAsync(List<OrderItem> entity, CancellationToken cancellationToken = default);


    }
}
