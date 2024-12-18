using System;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        public DateTime BookingDate { get; set; }

        [Required]
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }  // Navigationsproperty till Hotel
    }
}
