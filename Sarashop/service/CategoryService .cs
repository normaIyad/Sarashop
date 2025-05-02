using Sarashop.DataBase;
using Sarashop.Models;
using Sarashop.service;
using Sarashop.service.IServices;

namespace Sarashop.Service
{
    //ICatigoryService
    public class CategoryService : Service<Category>, ICatigoryService
    {
        private readonly DatabaseConfigration _databaseConfigration;

        public CategoryService(DatabaseConfigration databaseConfigration) : base(databaseConfigration)
        {
            _databaseConfigration = databaseConfigration;
        }

        async Task<bool> ICatigoryService.StatsChnage(int id, Category? category, CancellationToken cancellationToken)
        {
            Category category1 = _databaseConfigration.Categories.Find(id);
            if (category1 == null)
            {
                return false;
            }
            category1.State = !category1.State;
            await _databaseConfigration.SaveChangesAsync(cancellationToken);
            return true;
        }

        async Task<bool> ICatigoryService.Update(int id, Category? category, CancellationToken cancellationToken = default)
        {
            Category category1 = _databaseConfigration.Categories.Find(id);
            if (category1 == null)
            {
                return false;
            }
            category1.Name = category.Name;
            category1.Description = category.Description;
            await _databaseConfigration.SaveChangesAsync(cancellationToken);
            return true;

        }
    }
}
