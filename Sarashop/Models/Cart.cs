namespace Sarashop.Models
{
    public class Cart
    {
        public int ProductId { get; set; }
        public Prodact Product { get; set; }
        public string ApplecationUserId { get; set; }
        public ApplecationUser ApplecationUser { get; set; }
        public int Count { get; set; }
    }
}
