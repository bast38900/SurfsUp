using System.ComponentModel.DataAnnotations;

namespace SurfsUp.Models
{
    public class Board
    {
        public Guid BoardId { get; set; }

        [StringLength(255, MinimumLength = 1)]
        [Required]
        public string BoardName { get; set; }

        private string? picture;
        public string? Picture
        {
            get { return picture; }
            set { picture = value ?? ""; }
        }

        public double Lenght { get; set; }

        public double Width { get; set; }

        public double Thickness { get; set; }

        public double Volume { get; set; }

        [StringLength(255, MinimumLength = 1)]
        [Required]
        public string Type { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }


        /// <summary>
        /// Removes validation through null coalescing operator -> se set.
        /// </summary>
        private string? equipment;
        public string? Equipment
        {
            get { return equipment; }
            set { equipment = value ?? ""; }
        }

    }
}
