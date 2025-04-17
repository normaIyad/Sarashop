using Sarashop.DataBase;
using Sarashop.Models;
using System.Linq.Expressions;

namespace Sarashop.service
{
    public class BrandService : IBrand
    {
        private readonly DatabaseConfigration _databaseConfigration;

        public BrandService(DatabaseConfigration databaseConfigration)
        {
            _databaseConfigration = databaseConfigration;
        }

        public Brand GetBrand(Expression<Func<Brand, bool>> expression)
        {
            return _databaseConfigration.Brands.FirstOrDefault(expression);

        }

        public IEnumerable<Brand> GetBrands()
        {
            return _databaseConfigration.Brands.ToList<Brand>();

        }

        public IEnumerable<Brand> GetCategories()
        {
            throw new NotImplementedException();
        }

        Brand IBrand.Add(Brand brand)
        {
            _databaseConfigration.Brands.Add(brand);
            _databaseConfigration.SaveChanges();
            return brand;

        }

        bool IBrand.Delete(int id)
        {
            Brand brand = _databaseConfigration.Brands.FirstOrDefault(c => c.Id == id);
            if (brand == null)
            {
                return false;
            }
            _databaseConfigration.Brands.Remove(brand);
            _databaseConfigration.SaveChanges();
            return true;
        }

        bool IBrand.Update(int id, Brand br)
        {
            var brand = _databaseConfigration.Brands.Find(id);
            if (brand == null)
                return false;

            brand.Name = br.Name;
            brand.Description = br.Description;
            brand.State = br.State;
            _databaseConfigration.SaveChanges();
            return true;
        }
    }
}
