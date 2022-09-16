using System.ComponentModel.DataAnnotations.Schema;

namespace SurfsUp.Models
{

    public enum RentState
    {
       RentedOut, RentFinished
    }

    public class Rent
    {
        public Guid RentId { get; set; }
        public DateTime StartRent { get; set; }
        public DateTime EndRent { get; set; }
        public decimal Total { get; set; }
        public RentState RentState { get; set; }

        [ForeignKey("BoardId")]
        public Board Board { get; set; }
    }
}
