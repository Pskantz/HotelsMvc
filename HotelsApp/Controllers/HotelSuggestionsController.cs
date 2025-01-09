using Microsoft.AspNetCore.Mvc;
using HotelApp.Data;
using HotelApp.Models;
using System.Linq;

namespace HotelApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelSuggestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HotelSuggestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetSuggestions(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest("Search term cannot be empty.");
            }

            var suggestions = _context.Hotels
                .Where(h => h.Name.Contains(searchTerm) || h.Location.Contains(searchTerm))
                .Select(h => new { h.Id, h.Name, h.Location, h.Price })
                .ToList();

            return Ok(suggestions);
        }
    }
}