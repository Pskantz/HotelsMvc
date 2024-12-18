using System.ComponentModel.DataAnnotations;

namespace HotelsApp.Models;

public class BookViewModel
{
    public string Name { get; set; }

    public string Email { get; set; }

    public int NumOfRooms { get; set; }

    public Hotel Hotel { get; set; }
}