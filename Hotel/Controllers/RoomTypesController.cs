﻿using System;
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
    public class RoomTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
              return _context.RoomTypes != null ? 
                          View(await _context.RoomTypes
                          .Include(b => b.Beds)
                            .Include(a => a.Amenities).ToListAsync()) :
                          Problem("Entity set 'HotelHiddenKingdomContext.Room'  is null.");
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.RoomTypes == null)
            {
                return NotFound();
            }

            var room = await _context.RoomTypes
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
            RoomBedsAmens roomBedsAmens = new RoomBedsAmens();
            roomBedsAmens.Room = new RoomType();
            roomBedsAmens.Amenities = _context.Amenities.ToList();
            roomBedsAmens.Beds = _context.Beds.ToList();

            return View(roomBedsAmens);
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Id,Name,Description,BasePrice")] RoomType room, String[] bedList, String[] amenList)
        {
            foreach (String bedID in bedList)
            {
                Guid bedGUID = Guid.Parse(bedID);

                Bed bed = _context.Beds.Where(id => id.Id == bedGUID).First();
                room.Beds.Add(bed);
            }

            //Add all the selected amenities into the model 
            foreach (String amenID in amenList)
            {
                Guid amenGUID = Guid.Parse(amenID);

                Amenity amenity = _context.Amenities.Find(amenGUID);
                room.Amenities.Add(amenity);
            }

            if (ModelState.IsValid)
            {
                room.Id = Guid.NewGuid();
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.RoomTypes == null)
            {
                return NotFound();
            }

            var room = await _context.RoomTypes.Include(b => b.Beds).Include(a => a.Amenities).Where(a => a.Id == id).FirstAsync();
            if (room == null)
            {
                return NotFound();
            }
            RoomBedsAmens roomBedsAmens = new RoomBedsAmens();
            roomBedsAmens.Room = room;
            roomBedsAmens.Amenities = _context.Amenities.ToList();
            roomBedsAmens.Beds = _context.Beds.ToList();

            return View(roomBedsAmens);
            //return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,BasePrice")] RoomType room, String[] bedList, String[] amenList)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            RoomType roomToBeUpdated = _context.RoomTypes
                .Include(a => a.Amenities)
                .Include(b => b.Beds)
                .Where(i => i.Id == room.Id).First();

            roomToBeUpdated.Amenities.Clear();
            roomToBeUpdated.Beds.Clear();
            roomToBeUpdated.Name = room.Name;
            roomToBeUpdated.Description = room.Description;
            roomToBeUpdated.BasePrice = room.BasePrice;

            foreach (String bedID in bedList)
            {
                Guid bedGUID = Guid.Parse(bedID);
                Bed bed = _context.Beds.Find(bedGUID);
                roomToBeUpdated.Beds.Add(bed);
            }

            //Add all the selected amenities into the model 
            foreach (String amenID in amenList)
            {
                Guid amenGUID = Guid.Parse(amenID);

                Amenity amenity = _context.Amenities.Find(amenGUID);
                roomToBeUpdated.Amenities.Add(amenity);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomToBeUpdated);
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
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (_context.RoomTypes == null)
            {
                return Problem("Entity set 'HotelHiddenKingdomContext.Room'  is null.");
            }
            var room = await _context.RoomTypes.FindAsync(id);
            if (room != null)
            {
                _context.RoomTypes.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /*
        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Room == null)
            {
                return Problem("Entity set 'HotelHiddenKingdomContext.Room'  is null.");
            }
            var room = await _context.Room.FindAsync(id);
            if (room != null)
            {
                _context.Room.Remove(room);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */
        private bool RoomExists(Guid id)
        {
          return (_context.RoomTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}