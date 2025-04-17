using Sarashop.Models;
using System.Linq.Expressions;

namespace Sarashop.service
{
    public interface ICatigoryService
    {
        IEnumerable<Category> GetCategories();
        Category GetCategory(Expression<Func<Category, bool>> expression);
        Category Add(Category? category);
        bool Update(int id, Category? category);
        bool Delete(int id);
    }
}
