using Sarashop.Models;
using System.Linq.Expressions;

namespace Sarashop.service
{
    public interface IProdact
    {
        IEnumerable<Prodact> GetAll(string? query, int page, int limt);
        Prodact GetProdact(Expression<Func<Prodact, bool>> expression);
        Prodact Add(Prodact prodact);
        Prodact Update(int id, Prodact prodact);
        bool Delete(int id);
    }
}
