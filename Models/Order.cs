namespace SurfsUp.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public DateTime DateOfDelivery { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public Board? Board { get; set; }
        public decimal Total { get; set; }

    }
}
