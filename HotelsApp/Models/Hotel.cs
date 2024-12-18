using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty; // Standardvärde

        [Required]
        [StringLength(255)]
        public string ImageUrl { get; set; } = string.Empty; // Standardvärde

        public bool IsBooked { get; set; }
    }
}
