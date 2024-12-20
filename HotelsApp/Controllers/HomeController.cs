// filepath: /C:/Users/patri/Documents/GitHub/HotelsMvc/HotelsApp/Controllers/HomeController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelApp.Data;
using HotelApp.Models;

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
        [Authorize]
        public IActionResult AddHotel()
        {
            if (User.Identity?.Name == "admin@exampel.com")
            {
                return View();
            }
            return Forbid();
        }

        // Skapa hotell - POST
        [HttpPost]
        [Authorize]
        public IActionResult AddHotel(Hotel hotel)
        {
            if (User.Identity?.Name == "admin@exampel.com")
            {
                if (ModelState.IsValid)
                {
                    _context.Hotels.Add(hotel);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(hotel);
            }
            return Forbid();
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

        // Boka hotell - GET
        public IActionResult BookHotel(int id)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            var model = new BookViewModel
            {
                Hotel = hotel.Name,
                Price = hotel.Price
            };
            return View(model);
        }

        // Boka hotell - POST
        [HttpPost]
        public IActionResult BookHotel(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customerName = User.Identity?.Name ?? string.Empty;
                var nights = (model.CheckOut - model.CheckIn).Days;
                var totalPrice = model.Price * nights;
                var booking = new Booking
                {
                    CustomerName = customerName,
                    CheckIn = DateTime.SpecifyKind(model.CheckIn, DateTimeKind.Utc),
                    CheckOut = DateTime.SpecifyKind(model.CheckOut, DateTimeKind.Utc),
                    // Hotel = model.Hotel,
                    RoomType = model.RoomType,
                    Price = totalPrice
                };
                _context.Bookings.Add(booking);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Booking confirmed!";
                return RedirectToAction("BookedHotels");
            }
            return View(model);
        }

        // Visa bokade hotell
        public IActionResult BookedHotels()
        {
            var userName = User.Identity?.Name;
            var bookings = _context.Bookings.Where(b => b.CustomerName == userName).ToList();
            return View(bookings);
        }

        // Redigera bokning - GET
        public IActionResult EditBooking(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // Redigera bokning - POST
        [HttpPost]
        public IActionResult EditBooking(Booking booking)
        {
            // var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (ModelState.IsValid)
            {
                booking.CheckIn = DateTime.SpecifyKind(booking.CheckIn, DateTimeKind.Utc);
                booking.CheckOut = DateTime.SpecifyKind(booking.CheckOut, DateTimeKind.Utc);
                _context.Bookings.Update(booking);
                _context.SaveChanges();
                return RedirectToAction("BookedHotels");
            }
            return View(booking);
        }

        // Ta bort bokning
        [HttpPost]
        public IActionResult DeleteBooking(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null)
            {
                return NotFound();
            }
            _context.Bookings.Remove(booking);
            _context.SaveChanges();
            return RedirectToAction("BookedHotels");
        }

        // Ta bort hotell
        [HttpPost]
        public IActionResult DeleteHotel(int id)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            _context.Hotels.Remove(hotel);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}