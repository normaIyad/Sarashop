namespace Sarashop.DTO
{
    public class CartRequest
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string mainImg { get; set; }
        public string ApplecationUserId { get; set; }

    }
}
