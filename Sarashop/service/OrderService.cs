using Sarashop.DataBase;
using Sarashop.Models;
using Sarashop.service.IServices;

namespace Sarashop.service
{
    public class OrderService : Service<Order>, IOrderService
    {
        private readonly DatabaseConfigration databaseConfigration;

        public OrderService(DatabaseConfigration databaseConfigration) : base(databaseConfigration)
        {
            this.databaseConfigration = databaseConfigration;
        }


    }
}
