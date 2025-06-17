using Microsoft.EntityFrameworkCore;
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
        public async Task<Cart> AddToCart(string userId, int productid, CancellationToken cancellationToken)
        {
            var exsistinCart = await _databaseConfigration.Carts.FirstOrDefaultAsync(e => e.ApplecationUserId == userId && e.ProductId == productid);
            if (exsistinCart != null)
            {
                exsistinCart.Count++;
            }
            else
            {
                exsistinCart = new Cart
                {
                    ApplecationUserId = userId,
                    ProductId = productid,
                    Count = 1
                };
                await _databaseConfigration.Carts.AddAsync(exsistinCart, cancellationToken);
            }

            await _databaseConfigration.SaveChangesAsync(cancellationToken);

            return exsistinCart;
        }

        public async Task<IEnumerable<Cart>> ShowUserCart(string userId)
        {
            //            return await GetAsync(
            //    e => e.ApplicationUserId == userId,
            //    include: new Expression<Func<Cart, object>>[] { c => c.Product }
            //);
            return await GetAsync(e => e.ApplecationUserId == userId, inclode: [c => c.Product]);
        }
        public async Task<bool> RemoveRangeAsync(List<Cart> items, CancellationToken cancellationToken = default)
        {

            _databaseConfigration.RemoveRange(items);
            await _databaseConfigration.SaveChangesAsync(cancellationToken);
            return true;
        }


    }
}
