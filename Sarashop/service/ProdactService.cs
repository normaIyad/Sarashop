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

        IEnumerable<Prodact> IProdact.GetAll(string query, int page, int limt)
        {
            IQueryable<Prodact> prodacts = database.Prodacts;
            if (query != null)
            {
                prodacts = prodacts.Where(prodact => prodact.Name.Contains(query) || prodact.Description.Contains(query));
            }
            if (page <= 0 || limt <= 0)
            {
                page = 1;
                limt = 10;
            }
            prodacts = prodacts.Skip((page - 1) * limt).Take(limt);
            return prodacts.ToList();
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
