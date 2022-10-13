namespace SurfsUpAPI.Models
{
    public class RentDto
    {
        public Guid UserId { get; set; }
        public Guid BoardId { get; set; }
        public DateTime EndRent { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
