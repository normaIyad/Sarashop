namespace Sarashop.Models
{

    public class Review
    {
        public int Id { get; set; }
        public string ApplecationUserID { get; set; }
        public ApplecationUser ApplecationUser { get; set; }
        public int ProdactId { get; set; }
        public Prodact Prodact { get; set; }
        public int Rate { get; set; }
        public string? Comment { get; set; }
        public ICollection<ReviewImgs> ReviewImgs { get; } = new List<ReviewImgs>();
        public DateTime? ReviewDate { get; set; }
    }
}
