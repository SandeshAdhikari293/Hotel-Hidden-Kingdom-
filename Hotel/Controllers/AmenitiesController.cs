using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Models;
using System.Drawing;

namespace HotelHiddenKingdom.Controllers
{
    public class AmenitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AmenitiesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;

        }

        // GET: Amenities
        public async Task<IActionResult> Index()
        {
              return _context.Amenities != null ? 
                          View(await _context.Amenities.ToListAsync()) :
                          Problem("Entity set 'HotelHiddenKingdomContext.Amenity'  is null.");
        }

        // GET: Amenities/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Amenities == null)
            {
                return NotFound();
            }

            var amenity = await _context.Amenities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (amenity == null)
            {
                return NotFound();
            }

            return View(amenity);
        }

        // GET: Amenities/Create
        public IActionResult Add()
        {
            return View();
        }

        // POST: Amenities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Id,Name,Description")] Amenity amenity, IFormFile icon)
        {

            if (ModelState.IsValid)
            {

                string uploads = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot/AmenityIcons");
                //Create directory if it doesn't exist 
                Directory.CreateDirectory(uploads);
                string filePath = Path.Combine(uploads, icon.FileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    icon.CopyTo(fileStream);
                }

                amenity.IconPath = icon.FileName; 

                amenity.Id = Guid.NewGuid();
                _context.Add(amenity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(amenity);
        }

        // GET: Amenities/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Amenities == null)
            {
                return NotFound();
            }

            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity == null)
            {
                return NotFound();
            }
            return View(amenity);
        }

        // POST: Amenities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description")] Amenity amenity, IFormFile icon)
        {
            if (id != amenity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string uploads = Path.Combine(_hostEnvironment.ContentRootPath, "AmenityIcons");
                    //Create directory if it doesn't exist 
                    Directory.CreateDirectory(uploads);
                    string filePath = Path.Combine(uploads, icon.FileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        icon.CopyTo(fileStream);
                    }

                    amenity.IconPath = icon.FileName;


                    _context.Update(amenity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmenityExists(amenity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(amenity);
        }

        // GET: Amenities/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (_context.Amenities == null)
            {
                return Problem("Entity set 'HotelHiddenKingdomContext.Amenity'  is null.");
            }
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity != null)
            {
                _context.Amenities.Remove(amenity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /*// POST: Amenities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Amenity == null)
            {
                return Problem("Entity set 'HotelHiddenKingdomContext.Amenity'  is null.");
            }
            var amenity = await _context.Amenity.FindAsync(id);
            if (amenity != null)
            {
                _context.Amenity.Remove(amenity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool AmenityExists(Guid id)
        {
          return (_context.Amenities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
