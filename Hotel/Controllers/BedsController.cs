using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelHiddenKingdom.Controllers
{
    public class BedsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BedsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Beds
        public async Task<IActionResult> Index()
        {
              return _context.Beds != null ? 
                          View(await _context.Beds.ToListAsync()) :
                          Problem("Entity set 'HotelHiddenKingdomContext.Bed'  is null.");
        }

        // GET: Beds/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Beds == null)
            {
                return NotFound();
            }

            var bed = await _context.Beds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bed == null)
            {
                return NotFound();
            }

            return View(bed);
        }

        // GET: Beds/Create
        public IActionResult Add()
        {
            return View();
        }

        // POST: Beds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Id,Name,MaxPeople")] Bed bed, String[] bedList, String[] amenList)
        {
            bed.Rooms = new List<RoomType>();
             if (ModelState.IsValid)
            {
                bed.Id = Guid.NewGuid();
                _context.Add(bed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Beds/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Beds == null)
            {
                return NotFound();
            }

            var bed = await _context.Beds.FindAsync(id);
            if (bed == null)
            {
                return NotFound();
            }
            return View(bed);
        }

        // POST: Beds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,MaxPeople")] Bed bed)
        {
            if (id != bed.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BedExists(bed.Id))
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
            return View(bed);
        }

        // GET: Beds/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (_context.Beds == null)
            {
                return Problem("Entity set 'HotelHiddenKingdomContext.Bed'  is null.");
            }
            var bed = await _context.Beds.FindAsync(id);
            if (bed != null)
            {
                _context.Beds.Remove(bed);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /*
        // POST: Beds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Bed == null)
            {
                return Problem("Entity set 'HotelHiddenKingdomContext.Bed'  is null.");
            }
            var bed = await _context.Bed.FindAsync(id);
            if (bed != null)
            {
                _context.Bed.Remove(bed);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */
        private bool BedExists(Guid id)
        {
          return (_context.Beds?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
