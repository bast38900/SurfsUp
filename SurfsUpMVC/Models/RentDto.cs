using SurfsUp.Models;

namespace SurfsUpMVC.Models
{
    public class RentDto
    {
        public Guid UserId { get; set; }
        public Guid BoardId { get; set; }
        public DateTime EndRent { get; set; }
    }
}
