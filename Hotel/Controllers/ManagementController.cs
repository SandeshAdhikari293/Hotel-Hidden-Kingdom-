using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    public class ManagementController : Controller
    {
        ApplicationDbContext _context;
        public ManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(_context.Bookings.Include(r => r.Rooms).ThenInclude(rt => rt.Room.RoomType).ToList());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Details()
        {
            return View(CurrentBookings());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Booking(Guid id)
        {
            return View(_context.Bookings.Include(p => p.Personal).ThenInclude(a => a.Address)
                .Include(r => r.Rooms).ThenInclude(rt => rt.Room.RoomType).Where(i => i.Id == id).FirstOrDefault());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddBooking()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult BookingDetailsPartial(int state)
        {
            List<Booking> bookings = new List<Booking>();

            if(state == (int)BookingFilter.PREV)
            {
                bookings = PreviousBookings();
            }
            else if (state == (int)BookingFilter.CUR)
            {
                bookings = CurrentBookings();

            }
            else if (state == (int)BookingFilter.CIN)
            {
                bookings = Checkins();

            }
            else if (state == (int)BookingFilter.OUT)
            {
                bookings = Checkouts();

            }
            else if (state == (int)BookingFilter.ALL)
            {
                bookings = AllBookingsEver();

            }
            else if (state == (int)BookingFilter.STA)
            {
                bookings = StayingBookings();

            }
            else if (state == (int)BookingFilter.UPC)
            {
                bookings = UpcomingBookings();

            }
            
            FilterBooking filterBooking = new FilterBooking();
            filterBooking.bookings = bookings;
            filterBooking.state = state;

            return PartialView("_Bookings", filterBooking);
        }

        public List<Booking> CurrentBookings()
        {
            DateTime today = DateTime.Now.Date;
            //Returns a list of the current bookings - i.e. where the customer is staying at the hotel
            List<Booking> bookings = _context.Bookings
                .Include(p => p.Personal).ThenInclude(a => a.Address)
                .Include(r => r.Rooms).ThenInclude(rt => rt.Room.RoomType)
                .Where(cin => cin.CheckIn <= today && cin.CheckOut >= today).ToList();
            return bookings;
        }

        public List<Booking> UpcomingBookings()
        {
            DateTime today = DateTime.Now.Date;
            //Returns a list of the current bookings - i.e. where the customer is staying at the hotel
            List<Booking> bookings = _context.Bookings
                .Include(p => p.Personal).ThenInclude(a => a.Address)
                .Include(r => r.Rooms).ThenInclude(rt => rt.Room.RoomType)
                .Where(cin => cin.CheckIn > today).ToList();
            return bookings;
        }
        public List<Booking> PreviousBookings()
        {

                DateTime today = DateTime.Now.Date;
            //Returns a list of the current bookings - i.e. where the customer is staying at the hotel
            List<Booking> bookings = _context.Bookings
                .Include(p => p.Personal).ThenInclude(a => a.Address)
                .Include(r => r.Rooms).ThenInclude(rt => rt.Room.RoomType)
                .Where(cin => cin.CheckOut < today).ToList();

            return bookings;
        }

        public List<Booking> StayingBookings()
        {
            DateTime today = DateTime.Now.Date;
            //Returns a list of the current bookings - i.e. where the customer is staying at the hotel
            List<Booking> bookings = _context.Bookings
                .Include(p => p.Personal).ThenInclude(a => a.Address)
                .Include(r => r.Rooms).ThenInclude(rt => rt.Room.RoomType)
                .Where(cin => cin.CheckIn < today && cin.CheckOut > today).ToList();
            return bookings;
        }

        public List<Booking> AllBookingsEver ()
        {
            DateTime today = DateTime.Now.Date;
            //Returns a list of the current bookings - i.e. where the customer is staying at the hotel
            List<Booking> bookings = _context.Bookings
                .Include(p => p.Personal).ThenInclude(a => a.Address)
                .Include(r => r.Rooms).ThenInclude(rt => rt.Room.RoomType)
                .ToList();
            return bookings;
        }

        public List<Booking> Checkins()
        {
            DateTime today = DateTime.Now.Date;
            //Returns a list of the current bookings - i.e. where the customer is staying at the hotel
            List<Booking> bookings = _context.Bookings
                .Include(p => p.Personal).ThenInclude(a => a.Address)
                .Include(r => r.Rooms).ThenInclude(rt => rt.Room.RoomType)
                .Where(cin => cin.CheckIn == today).ToList();
            return bookings;
        }

        public List<Booking> Checkouts()
        {
            DateTime today = DateTime.Now.Date;
            //Returns a list of the current bookings - i.e. where the customer is staying at the hotel
            List<Booking> bookings = _context.Bookings
                .Include(p => p.Personal).ThenInclude(a => a.Address)
                .Include(r => r.Rooms).ThenInclude(rt => rt.Room.RoomType)
                .Where(cin => cin.CheckOut == today).ToList();
            return bookings;
        }

        public IActionResult SidepanelPartial()
        {
            return PartialView("_Sidepanel");
        }
    }
}
