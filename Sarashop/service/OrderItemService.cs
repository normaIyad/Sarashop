using Sarashop.DataBase;
using Sarashop.Models;
using Sarashop.service.IServices;

namespace Sarashop.service
{
    public class OrderItemService : Service<OrderItem>, IOrderItem
    {
        private readonly DatabaseConfigration databaseConfigration;

        public OrderItemService(DatabaseConfigration databaseConfigration) : base(databaseConfigration)
        {
            this.databaseConfigration = databaseConfigration;
        }
        public async Task<List<OrderItem>> AddMnayAsync(List<OrderItem> entity, CancellationToken cancellationToken = default)
        {
            await databaseConfigration.AddRangeAsync(entity, cancellationToken);
            await databaseConfigration.SaveChangesAsync(cancellationToken);
            return entity;
        }


    }
}
