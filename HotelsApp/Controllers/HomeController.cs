using HotelApp.Data;
using HotelApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Visa alla hotell
        public IActionResult Index()
        {
            var hotels = _context.Hotels.ToList();
            return View(hotels);
        }

        // Skapa hotell - GET
        public IActionResult AddHotel()
        {
            return View();
        }

        // Skapa hotell - POST
        [HttpPost]
        public IActionResult AddHotel(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _context.Hotels.Add(hotel);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hotel);
        }

        // Redigera hotell - GET
        public IActionResult EditHotel(int id)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // Redigera hotell - POST
        [HttpPost]
        public IActionResult EditHotel(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _context.Hotels.Update(hotel);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hotel);
        }

        // Boka hotell
        [HttpPost]
        public IActionResult BookHotel(int id)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            hotel.IsBooked = true;
            _context.Hotels.Update(hotel);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Visa bokade hotell
        public IActionResult BookedHotels()
        {
            var bookedHotels = _context.Hotels.Where(h => h.IsBooked).ToList();
            return View(bookedHotels);
        }

        public IActionResult Administrator()
        {
            return View();
        }
    }
}
