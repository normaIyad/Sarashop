namespace Sarashop.DTO
{
    public class BrandREQ
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool State { get; set; }
        public IFormFile mainImg { get; set; }
    }
}
