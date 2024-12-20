using System;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public Hotel? Hotel { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}