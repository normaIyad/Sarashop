using Sarashop.DataBase;
using Sarashop.Models;
using Sarashop.service.IServices;

namespace Sarashop.service
{
    public class ReviewService : Service<Review>, IReview
    {
        private readonly DatabaseConfigration databaseConfigration;

        public ReviewService(DatabaseConfigration databaseConfigration) : base(databaseConfigration)
        {
            this.databaseConfigration = databaseConfigration;
        }

        //public Task<bool> Update(int id, Review review, CancellationToken cancellationToken = default)
        //{
        //    return false;
        //}
    }
}
