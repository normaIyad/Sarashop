using Sarashop.Models;
using Sarashop.service.IServices;

namespace Sarashop.service
{
    public interface IBrand : IService<Brand>
    {
        bool Update(int id, Brand brand);
        bool state(int id, Brand brand);
    }
}

