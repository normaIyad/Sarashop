using Sarashop.DataBase;
using Sarashop.Models;
using Sarashop.service.IServices;

namespace Sarashop.service
{
    public class BrandService : Service<Brand>, IBrand
    {
        private readonly DatabaseConfigration _databaseConfigration;

        public BrandService(DatabaseConfigration databaseConfigration) : base(databaseConfigration)
        {
            _databaseConfigration = databaseConfigration;
        }
        bool IBrand.Update(int id, Brand br)
        {
            var brand = _databaseConfigration.Brands.Find(id);
            if (brand == null)
                return false;

            brand.Name = br.Name;
            brand.Description = br.Description;
            _databaseConfigration.SaveChanges();
            return true;
        }
        bool IBrand.state(int id, Brand brand)
        {
            var brand_database = _databaseConfigration.Brands.Find(id);
            if (brand == null)
                return false;

            brand_database.State = !brand_database.State;
            _databaseConfigration.SaveChanges();
            return true;
        }
    }
}
