using HotelApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HotelApp.Controllers
{
    public class HomeController : Controller
    {
        // Temporär lista för att lagra hotell
        private static List<Hotel> _hotels = new List<Hotel>();

        // Visa startsidan med alla hotell
        public IActionResult Index()
        {
            return View(_hotels); // Skicka listan med hotell till vyn
        }

        // Visa formulär för att lägga till hotell
        public IActionResult AddHotel()
        {
            return View();
        }

        // Hantera indata från formuläret
        [HttpPost]
        public IActionResult AddHotel(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _hotels.Add(hotel); // Lägg till hotellet i listan
                return RedirectToAction("Index"); // Skicka användaren till startsidan
            }

            return View(hotel); // Om något är fel, visa formuläret igen
        }
    }
}
