using Sarashop.Models;
using Sarashop.service.IServices;

namespace Sarashop.service
{
    public interface IReview : IService<Review>
    {
        // Task<bool> Update(int id, Review review, CancellationToken cancellationToken = default);

    }
}
