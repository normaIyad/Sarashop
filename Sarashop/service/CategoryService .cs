using Sarashop.DataBase;
using Sarashop.Models;
using Sarashop.service;
using System.Linq.Expressions;

namespace Sarashop.Service
{
    public class CategoryService : ICatigoryService
    {
        private readonly DatabaseConfigration _databaseConfigration;

        public CategoryService(DatabaseConfigration databaseConfigration)
        {
            _databaseConfigration = databaseConfigration;
        }

        public Category Add(Category category)
        {
            _databaseConfigration.Categories.Add(category);
            _databaseConfigration.SaveChanges();
            return category;
        }

        public bool Delete(int id)
        {
            var category = _databaseConfigration.Categories.Find(id);
            if (category == null)
                return false;

            _databaseConfigration.Categories.Remove(category);
            _databaseConfigration.SaveChanges();
            return true;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _databaseConfigration.Categories.ToList();
        }

        public Category GetCategory(Expression<Func<Category, bool>> expression)
        {
            return _databaseConfigration.Categories.FirstOrDefault(expression);
        }

        public bool Update(int id, Category category)
        {
            var cat = _databaseConfigration.Categories.Find(id);
            if (cat == null)
                return false;

            cat.Name = category.Name;
            cat.Description = category.Description;
            cat.State = category.State;

            _databaseConfigration.SaveChanges();
            return true;
        }
    }
}
