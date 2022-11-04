namespace SurfsUpLibrary.Models
{
    public class RentDto
    {
        public Guid UserId { get; set; }
        public Guid BoardId { get; set; }
        public DateTime EndRent { get; set; }
    }
}
