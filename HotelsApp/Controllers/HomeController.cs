using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelApp.Data;
using HotelApp.Models;
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

        public IActionResult Index(string searchString, decimal? minPrice, decimal? maxPrice)
{
    ViewData["CurrentFilter"] = searchString;
    ViewData["MinPrice"] = minPrice;
    ViewData["MaxPrice"] = maxPrice;

    var hotels = from h in _context.Hotels
                 select h;

    if (!string.IsNullOrEmpty(searchString))
    {
        hotels = hotels.Where(s => s.Name.Contains(searchString) || s.Location.Contains(searchString));
    }

    if (minPrice.HasValue)
    {
        hotels = hotels.Where(h => h.Price >= minPrice.Value);
    }

    if (maxPrice.HasValue)
    {
        hotels = hotels.Where(h => h.Price <= maxPrice.Value);
    }

    return View(hotels.ToList());
}

        [Authorize]
        public IActionResult AddHotel()
        {
            if (User.Identity?.Name == "admin@exampel.com")
            {
                return View();
            }
            return Forbid();
        }

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

        public IActionResult EditHotel(int id)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

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

        public IActionResult BookHotel(int id)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            var model = new BookViewModel
            {
                HotelId = id,
                Hotel = hotel.Name,
                Price = hotel.Price
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult BookHotel(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customerName = User.Identity?.Name ?? string.Empty;
                var nights = (model.CheckOut - model.CheckIn).Days;
                var hotel = _context.Hotels.FirstOrDefault(h => h.Id == model.HotelId);
                if (hotel == null)
                {
                    return NotFound();
                }

                decimal roomPrice = hotel.Price;
                switch (model.RoomType)
                {
                    case "Double":
                        roomPrice *= 1.2m;
                        break;
                    case "Suite":
                        roomPrice *= 1.5m;
                        break;
                }

                var totalPrice = roomPrice * nights;

                var booking = new Booking
                {
                    CustomerName = customerName,
                    CheckIn = DateTime.SpecifyKind(model.CheckIn, DateTimeKind.Utc),
                    CheckOut = DateTime.SpecifyKind(model.CheckOut, DateTimeKind.Utc),
                    HotelId = model.HotelId,
                    Hotel = hotel,
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

        public IActionResult BookedHotels()
        {
            var userName = User.Identity?.Name;
            var bookings = _context.Bookings.Include(b => b.Hotel).Where(b => b.CustomerName == userName).ToList();
            return View(bookings);
        }

        public IActionResult EditBooking(int id)
        {
            var booking = _context.Bookings.Include(b => b.Hotel).FirstOrDefault(b => b.Id == id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpPost]
        public IActionResult EditBooking(Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.CheckIn = DateTime.SpecifyKind(booking.CheckIn, DateTimeKind.Utc);
                booking.CheckOut = DateTime.SpecifyKind(booking.CheckOut, DateTimeKind.Utc);

                var nights = (booking.CheckOut - booking.CheckIn).Days;

                var hotel = _context.Hotels.FirstOrDefault(h => h.Id == booking.HotelId);
                if (hotel == null)
                {
                    return NotFound();
                }

                decimal roomPrice = hotel.Price;
                switch (booking.RoomType)
                {
                    case "Double":
                        roomPrice *= 1.2m;
                        break;
                    case "Suite":
                        roomPrice *= 1.5m;
                        break;
                }

                var totalPrice = roomPrice * nights;
                booking.Price = totalPrice;

                _context.Bookings.Update(booking);
                _context.SaveChanges();
                return RedirectToAction("BookedHotels");
            }

            return View(booking);
        }

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

        [HttpGet]
        public async Task<IActionResult> HotelSuggestions(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm) || searchTerm.Length < 3)
            {
                return BadRequest("Search term must be at least 3 characters long.");
            }

            var suggestions = await _context.Hotels
                .Where(h => h.Name.Contains(searchTerm) || h.Location.Contains(searchTerm))
                .Select(h => new { h.Id, h.Name, h.Location, h.Price, h.ImageUrl })
                .ToListAsync();

            return Json(suggestions);
        }
    }
}