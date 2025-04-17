using Sarashop.Models;
using System.Linq.Expressions;

namespace Sarashop.service
{
    public interface IBrand
    {
        IEnumerable<Brand> GetBrands();
        Brand GetBrand(Expression<Func<Brand, bool>> expression);
        Brand Add(Brand brand);
        bool Update(int id, Brand brand);
        bool Delete(int id);
        IEnumerable<Brand> GetCategories();
    }
}

