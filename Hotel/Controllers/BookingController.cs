using Hotel.Data;
using Hotel.Models;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Buffers.Text;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.Json;
using Newtonsoft.Json;
using System.Net;
using Hotel.Migrations;

namespace Hotel.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            //var roomList = HttpContext.Session.Get<List<Room>>("RoomList");

            _context = context;
        }

        public IActionResult Index()
        {
            //List<Booking> bookings = _context.Bookings.Include(p => p.Personal).ThenInclude(a => a.Address).Include(r => r.Rooms).ThenInclude(t => t.RoomType).ToList();
           // Debug.WriteLine(bookings);
            return View();
        }

        
        public IActionResult SearchBooking([Bind("Id,People,RoomCount")] Booking booking, string checkIn, string checkOut)
        {

            DateTime checkinDate = DateTime.ParseExact(checkIn, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime checkoutDate = DateTime.ParseExact(checkOut, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            BookingCart bookingCart = new BookingCart();

            booking.CheckIn = checkinDate;
            booking.CheckOut = checkoutDate;

            if (ModelState.IsValid)
            {
                bookingCart.Id = Guid.NewGuid();
                bookingCart.Booking = booking;
                bookingCart.CheckIn = checkinDate;
                bookingCart.CheckOut = checkoutDate;
                bookingCart.People = booking.People;
                bookingCart.RoomCount = booking.RoomCount;
            }

            BookingRooms bookingRooms = new BookingRooms();



            Dictionary<RoomType, List<Room>> RoomList = new Dictionary<RoomType, List<Room>>();

            foreach(Room room in _context.Rooms.Include(r => r.RoomType).ToList())
            {
                if (!isAvailable(room, checkinDate, checkoutDate)) continue;
                if (!RoomList.ContainsKey(room.RoomType))
                {
                    RoomList.Add(room.RoomType, new List<Room>());
                }
                RoomList[room.RoomType].Add(room);
            }

            bookingRooms.RoomList = RoomList;
            bookingRooms.BookingCart = bookingCart;


            //TempData["Booking"] = bookingRooms;
            HttpContext.Session.Remove("bookingcart");
            HttpContext.Session.SetObjectAsJson("bookingcart", bookingCart);

            return View("RoomSelection", bookingRooms);
        }
        /*
         *             BookingCart bookingCart = HttpContext.Session.GetObjectFromJson<BookingCart>("bookingcart");
            Booking booking = new Booking();

            booking.Id = Guid.NewGuid();
            booking.CheckIn = bookingCart.CheckIn;
            booking.CheckOut = bookingCart.CheckOut;
            booking.Rooms = bookingCart.SelectedRooms;
            booking.BookingDate = DateTime.Now;
            booking.People = bookingCart.People;
            booking.RoomCount = bookingCart.RoomCount;
         */
        public IActionResult BookingConfirmation()
        {
            BookingCart bookingCart = HttpContext.Session.GetObjectFromJson<BookingCart>("bookingcart");
            Booking booking = new Booking();

            booking.CheckIn = bookingCart.CheckIn;
            booking.CheckOut = bookingCart.CheckOut;
            booking.BookingDate = DateTime.Now;
            booking.People = bookingCart.People;
            booking.RoomCount = bookingCart.RoomCount;
            
            return View("Checkout", bookingCart);
        }

        public IActionResult Success()
        {
            return View();
        }


        public async Task<IActionResult> PlaceBooking(Booking booking)
        {
            BookingCart bookingCart = HttpContext.Session.GetObjectFromJson<BookingCart>("bookingcart");
            booking.BookingDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                foreach(BookedRoom room in bookingCart.SelectedRooms)
                {
                    Room roomToAdd = _context.Rooms.Find(room.Room.Id);
                    room.Room = roomToAdd;

                    booking.Rooms.Add(room);
                }

                booking.Id = Guid.NewGuid();
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Success));
            }
            return RedirectToAction(nameof(BookingConfirmation));
        }


        public async Task<IActionResult> ConfirmBooking(Booking booking)
        {
            BookingCart bookingCart = HttpContext.Session.GetObjectFromJson<BookingCart>("bookingcart");

            if (ModelState.IsValid)
            {
                foreach (BookedRoom room in bookingCart.SelectedRooms)
                {
                    Room roomToAdd = _context.Rooms.Find(room.Id);
                    room.Room = roomToAdd;

                    booking.Rooms.Add(room);
                }

                booking.Id = Guid.NewGuid();
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Success));
            }
            return View(booking);
        }


        public bool isAvailable(Room room, DateTime checkin, DateTime checkout)
        {
            foreach (Booking booking in _context.Bookings.Include(r => r.Rooms).Where(cin => 
            cin.CheckIn <= DateTime.Today.Date && cin.CheckOut >= DateTime.Today.Date).ToList())
            {
                foreach(BookedRoom bookedRoom in booking.Rooms)
                {
                    if(bookedRoom.Room.Id == room.Id)
                    {
                        bool overlap = booking.CheckIn < checkout && checkin < booking.CheckOut;
                        if (overlap)
                        {
                            if (!booking.Cancelled)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public IActionResult RoomsPartial()
        {
            BookingCart bookingCart = HttpContext.Session.GetObjectFromJson<BookingCart>("bookingcart");

            BookingRooms bookingRooms = new BookingRooms();
            bookingRooms.RoomList = getRoomList(bookingCart);
            bookingRooms.BookingCart = bookingCart;

            return PartialView("_Rooms2", bookingRooms);
        }



        public IActionResult InformationPartial()
        {
            BookingCart bookingCart = HttpContext.Session.GetObjectFromJson<BookingCart>("bookingcart");

            BookingRooms bookingRooms = new BookingRooms();
            bookingRooms.RoomList = getRoomList(bookingCart);
            bookingRooms.BookingCart = bookingCart;

            return PartialView("_Information", bookingRooms);
        }

        [HttpPost]
        public ActionResult RefreshInfo()
        {
            return RedirectToAction(nameof(InformationPartial));
        }

        public Dictionary<RoomType, List<Room>> getRoomList(BookingCart bookingCart)
        {
            Dictionary<RoomType, List<Room>> RoomList = new Dictionary<RoomType, List<Room>>();

            foreach (Room room in _context.Rooms.Include(r => r.RoomType).ThenInclude(b => b.Beds).ToList())
            {
                if (!isAvailable(room, bookingCart.CheckIn, bookingCart.CheckOut)) continue;
                if (!RoomList.ContainsKey(room.RoomType))
                {

                    RoomList.Add(room.RoomType, new List<Room>());

                }

                if (!contains(bookingCart.SelectedRooms, room))
                {
                    RoomList[room.RoomType].Add(room);
                }
                else
                {

                }
            }

            foreach (KeyValuePair<RoomType, List<Room>> entry in RoomList)
            {
                if (entry.Value.Count == 0)
                {
                    RoomList.Remove(entry.Key);
                }
            }

            return RoomList;
        }

        public bool contains(List<BookedRoom> rooms, Room room)
        {
            if(rooms == null) return false;
            foreach(BookedRoom r in rooms)
            {
                if (r.Room.Id.ToString().Equals(room.Id.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        public BookedRoom getBookedRoom(List<BookedRoom> rooms, Room room)
        {
            if (rooms == null) return null;
            foreach (BookedRoom r in rooms)
            {
                if (r.Room.Id.ToString().Equals(room.Id.ToString()))
                {
                    return r;
                }
            }
            return null;
        }


        [HttpPost]
        public ActionResult SubmitRoomSelection()
        {
            BookingCart bookingCart = HttpContext.Session.GetObjectFromJson<BookingCart>("bookingcart");

            if (bookingCart.SelectedRooms.Count >= bookingCart.RoomCount)
            {
                Response.StatusCode = 550;
                return Content("Errror");
            }

            return Ok();
        }
        

        [HttpPost]
        public ActionResult AddRoom(string roomID, int people)
        {
            BookingCart bookingCart = HttpContext.Session.GetObjectFromJson<BookingCart>("bookingcart");
            if(bookingCart == null) return RedirectToAction("Index");

            Room roomToAdd = _context.Rooms.Include(t => t.RoomType).ThenInclude(b => b.Beds).Where(id => id.Id == Guid.Parse(roomID)).FirstOrDefault();
                
                //Find(Guid.Parse(roomID));

            if(bookingCart.SelectedRooms.Count >= bookingCart.RoomCount)
            {
                Response.StatusCode = 550;
                return Content("Errror");
            }

            if(!contains(bookingCart.SelectedRooms, roomToAdd))
            {
                BookedRoom bookedRoom = new BookedRoom();
                bookedRoom.Room = roomToAdd;
                bookedRoom.Occupants = people;
                bookedRoom.Booking = bookingCart.Booking;
                bookingCart.SelectedRooms.Add(bookedRoom);
            }

            HttpContext.Session.Remove("bookingcart");
            HttpContext.Session.SetObjectAsJson("bookingcart", bookingCart);

            return RedirectToAction(nameof(RoomsPartial));
        }

        [HttpPost]
        public ActionResult RemoveRoom(string roomID)
        {
            BookingCart bookingCart = HttpContext.Session.GetObjectFromJson<BookingCart>("bookingcart");
            if (bookingCart == null) return RedirectToAction("Index");

            Room roomToAdd = _context.Rooms.Include(t => t.RoomType).ThenInclude(b => b.Beds).Where(id => id.Id == Guid.Parse(roomID)).FirstOrDefault();

            //Find(Guid.Parse(roomID));

            if (contains(bookingCart.SelectedRooms, roomToAdd))
            {
                bookingCart.SelectedRooms.Remove(getBookedRoom(bookingCart.SelectedRooms, roomToAdd));
            }

            HttpContext.Session.Remove("bookingcart");
            HttpContext.Session.SetObjectAsJson("bookingcart", bookingCart);

            return RedirectToAction(nameof(RoomsPartial));
        }


        public IActionResult AddRoomToBooking(string roomnumber, int people, bool breakfast)
        {
            BookingCart bookingCart = HttpContext.Session.GetObjectFromJson<BookingCart>("bookingcart");
            if (bookingCart == null) return RedirectToAction("Index");

            Room roomToAdd = _context.Rooms.Include(t => t.RoomType).ThenInclude(b => b.Beds).Where(id => id.Id == Guid.Parse(roomnumber)).FirstOrDefault();

            //Find(Guid.Parse(roomID));

            if (bookingCart.SelectedRooms.Count >= bookingCart.RoomCount)
            {
                Response.StatusCode = 550;
                return Content("Errror");
            }

            if (!contains(bookingCart.SelectedRooms, roomToAdd))
            {
                BookedRoom bookedRoom = new BookedRoom();
                bookedRoom.Room = roomToAdd;
                bookedRoom.Occupants = people;
                bookedRoom.Booking = bookingCart.Booking;
                bookedRoom.BreakfastIncluded = breakfast;
                bookingCart.SelectedRooms.Add(bookedRoom);
            }

            HttpContext.Session.Remove("bookingcart");
            HttpContext.Session.SetObjectAsJson("bookingcart", bookingCart);

            BookingCart bookingCart1 = HttpContext.Session.GetObjectFromJson<BookingCart>("bookingcart");

            int ppl = 0;
            foreach(BookedRoom booked in bookingCart1.SelectedRooms)
            {
                ppl = ppl + booked.Occupants;
            }
            Debug.WriteLine(ppl);

            return UpdateBooking();
        }

        public IActionResult UpdateBooking()
        {

            BookingCart bookingCart = HttpContext.Session.GetObjectFromJson<BookingCart>("bookingcart");
            if (bookingCart == null) return RedirectToAction("Index");

            BookingRooms bookingRooms = new BookingRooms();

            Dictionary<RoomType, List<Room>> RoomList = new Dictionary<RoomType, List<Room>>();

            foreach (Room room in _context.Rooms.Include(r => r.RoomType).ThenInclude(b => b.Beds).ToList())
            {
                if (!isAvailable(room, bookingCart.CheckIn, bookingCart.CheckOut)) continue;
                if (!RoomList.ContainsKey(room.RoomType))
                {

                    RoomList.Add(room.RoomType, new List<Room>());

                }

                if (!contains(bookingCart.SelectedRooms, room))
                {
                    RoomList[room.RoomType].Add(room);
                }
                else
                {

                }
            }

            foreach (KeyValuePair<RoomType, List<Room>> entry in RoomList)
            {
                if (entry.Value.Count == 0)
                {
                    RoomList.Remove(entry.Key);
                }
            }

            bookingRooms.RoomList = RoomList;
            bookingRooms.BookingCart = bookingCart;

            HttpContext.Session.Remove("bookingcart");
            HttpContext.Session.SetObjectAsJson("bookingcart", bookingCart);

            return View("RoomSelection", bookingRooms);
        }
    }
}
