using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    public class RoomInfoController : Controller
    {
        ApplicationDbContext _context;
        public RoomInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(Guid id)
        {
            RoomType roomType = _context.RoomTypes.Include(b => b.Beds).Include(a => a.Amenities).Include(img => img.Images).Where(i => i.Id == id).FirstOrDefault();
            return View(roomType);
        }
    }
}
