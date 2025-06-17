namespace Sarashop.Models
{
    public enum OrderStatus
    {
        pending,
        Cnacelled,
        Approved,
        Shipped,
        Completed
    }
    public enum PamentMethodeType
    {
        Visa, Chash
    }
    public class Order
    {
        //Order
        public int Id { get; set; }
        public OrderStatus orderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public decimal TotalPrice { get; set; }
        //Pay
        public PamentMethodeType pamentMethodeType { get; set; }
        public string? SessionId { get; set; }
        public string? TransactionID { get; set; }
        //Carrier
        public string? Carrier { get; set; }
        public string? TrackingNumber { get; set; }
        //Relation
        public string applecationUserId { get; set; }
        public ApplecationUser applecationUser { get; set; }
    }
}
