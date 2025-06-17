using Microsoft.EntityFrameworkCore;

namespace Sarashop.Models
{
    [PrimaryKey(nameof(OrderId), nameof(ProdactId))]
    public class OrderItem
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProdactId { get; set; }
        public Prodact Prodact { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }
    }
}
