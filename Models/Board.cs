namespace SurfsUp.Models
{
    public class Board
    {
        public Guid BoardId { get; set; }
        public string BoardName { get; set; }
        public double Lenght { get; set; }
        public double Width { get; set; }
        public double Thickness { get; set; }
        public double Volume { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public string Equipment { get; set; }
    }
}
