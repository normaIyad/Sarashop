using Sarashop.DataBase;
using Sarashop.Models;
using Sarashop.service.IServices;

namespace Sarashop.service
{
    public class CartServices : Service<Cart>, ICart
    {
        private readonly DatabaseConfigration _databaseConfigration;

        public CartServices(DatabaseConfigration databaseConfigration) : base(databaseConfigration)
        {
            _databaseConfigration = databaseConfigration;
        }


    }
}
