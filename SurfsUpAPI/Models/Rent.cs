using System.ComponentModel.DataAnnotations.Schema;

namespace SurfsUpAPI.Models
{
    public enum RentState
    {
        RentedOut, RentFinished
    }

    public class Rent
    {
        public Guid? RentId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime StartRent { get; set; }
        public DateTime EndRent { get; set; }
        public decimal Total { get; set; }
        public RentState RentState { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("BoardId")]
        public Board Board { get; set; }
    }
}
