using System.ComponentModel.DataAnnotations;
using HotelApp.Models;

namespace HotelApp.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsBooked { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
