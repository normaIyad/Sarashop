namespace Sarashop.DTO
{
    public class BrandDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool State { get; set; }
        public IFormFile mainImg { get; set; }
    }
}
