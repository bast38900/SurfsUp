namespace SurfsUp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime DateOfDelivery { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public decimal Total { get; set; }

    }
}
