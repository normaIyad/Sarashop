namespace Sarashop.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool State { get; set; }
        public string mainImg { get; set; }
        public ICollection<Prodact> Prodacts { get; set; }

    }
}
