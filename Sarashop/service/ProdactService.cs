using Sarashop.DataBase;
using Sarashop.Models;
using System.Linq.Expressions;

namespace Sarashop.service
{
    public class ProdactService : IProdact
    {
        private readonly DatabaseConfigration database;
        public ProdactService(DatabaseConfigration data)
        {
            database = data;
        }

        Prodact IProdact.Add(Prodact prodact)
        {
            database.Prodacts.Add(prodact);
            database.SaveChanges();
            return prodact;
        }

        bool IProdact.Delete(int id)
        {
            var c = database.Prodacts.Find(id);
            if (c == null)
                return false;
            database.Prodacts.Remove(c);
            database.SaveChanges();
            return true;
        }

        IEnumerable<Prodact> IProdact.GetAll()
        {
            return database.Prodacts.ToList();
        }

        Prodact IProdact.GetProdact(Expression<Func<Prodact, bool>> expression)
        {
            return database.Prodacts.FirstOrDefault(expression);
        }

        Prodact IProdact.Update(int id, Prodact prodact)
        {
            var c = database.Prodacts.Find(id);
            if (c == null)
                return null;
            c.Name = prodact.Name;
            c.State = prodact.State;
            c.Discount = prodact.Discount;
            c.Quntity = prodact.Quntity;
            c.Rate = prodact.Rate;
            c.Price = prodact.Price;
            c.Description = prodact.Description;
            c.Category = prodact.Category;
            database.SaveChanges();
            return prodact;
        }
    }
}
