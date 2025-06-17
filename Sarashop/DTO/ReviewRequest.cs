namespace Sarashop.DTO
{
    public class ReviewRequest
    {
        public int Rate { get; set; }
        public string? Comment { get; set; }
        public ICollection<ReviewImgRequest>? ReviewImgs { get; } = new List<ReviewImgRequest>();
        public DateTime? ReviewDate { get; set; }


    }
}
