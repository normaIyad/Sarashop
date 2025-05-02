using Sarashop.Models;
using Sarashop.service.IServices;

namespace Sarashop.service
{
    public interface ICatigoryService : IService<Category>
    {
        Task<bool> Update(int id, Category? category, CancellationToken cancellationToken = default);
        Task<bool> StatsChnage(int id, Category? category, CancellationToken cancellationToken = default);

    }
}
