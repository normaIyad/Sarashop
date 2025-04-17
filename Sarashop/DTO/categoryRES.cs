namespace Sarashop.DTO
{
    public class categoryRES
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool State { get; set; }
        public IFormFile mainImg { get; set; }

    }
}
