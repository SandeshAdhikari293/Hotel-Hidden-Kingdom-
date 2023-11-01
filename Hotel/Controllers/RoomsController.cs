using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Models;

namespace HotelHiddenKingdom.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
              return _context.Rooms != null ? 
                          View(await _context.Rooms.Include(r => r.RoomType).ToListAsync()) :
                          Problem("Entity set 'HotelHiddenKingdomContext.Room'  is null.");
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Add()
        {
            RoomRoomTypes roomRoomTypes = new RoomRoomTypes();
            roomRoomTypes.RoomTypes = _context.RoomTypes.ToList();

            return View(roomRoomTypes);
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Id,Number")] Room room, string roomType)
        {
            room.RoomType = _context.RoomTypes.Find(Guid.Parse(roomType));
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            RoomRoomTypes roomRoomTypes = new RoomRoomTypes();
            roomRoomTypes.Room = room;
            roomRoomTypes.RoomTypes = _context.RoomTypes.ToList();
            return View(roomRoomTypes);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            RoomRoomTypes roomRoomTypes = new RoomRoomTypes();
            roomRoomTypes.Room = room;
            roomRoomTypes.RoomTypes = _context.RoomTypes.ToList();

            return View(roomRoomTypes);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Number")] Room room, string roomType)
        {
            room.RoomType = _context.RoomTypes.Find(Guid.Parse(roomType));
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
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
            RoomRoomTypes roomRoomTypes = new RoomRoomTypes();
            roomRoomTypes.Room = room;
            roomRoomTypes.RoomTypes = _context.RoomTypes.ToList();

            return View(roomRoomTypes);
        }


        // POST: Rooms/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (_context.Rooms == null)

            {
                return Problem("Entity set 'HotelHiddenKingdomContext.Room'  is null.");
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(Guid id)
        {
          return (_context.Rooms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
