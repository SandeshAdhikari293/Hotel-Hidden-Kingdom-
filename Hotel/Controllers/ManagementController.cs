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

            Statistics statistics = new Statistics(GetActiveBookings());
            statistics.Rooms = _context.Rooms.ToList();
            return View(statistics);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Details()
        {
            return View(CurrentBookings());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Booking(Guid id)
        {
            return View(GetBooking(id));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddBooking()
        {
            return View();
        }

        public Booking GetBooking(Guid id)
        {
            return _context.Bookings.Include(p => p.Personal).ThenInclude(a => a.Address)
                .Include(r => r.Rooms).ThenInclude(rt => rt.Room.RoomType).Where(i => i.Id == id).FirstOrDefault();
        }
        public List<Booking> GetBookings()
        {
            return _context.Bookings.Include(p => p.Personal).ThenInclude(a => a.Address)
                .Include(r => r.Rooms).ThenInclude(rt => rt.Room.RoomType).ToList();
        }

        public List<Booking> GetActiveBookings()
        {
            return _context.Bookings.Include(p => p.Personal).ThenInclude(a => a.Address)
                .Include(r => r.Rooms).ThenInclude(rt => rt.Room.RoomType).Where(b => b.Cancelled == false).ToList();
        }

        public IActionResult CancelBooking(Guid id)
        {
            Booking booking = GetBooking(id);
            booking.Cancelled = true;

            _context.Update(booking);
            _context.SaveChanges();

            return View("Booking", GetBooking(id));
        }

        public IActionResult UncancelBooking(Guid id)
        {
            Booking booking = GetBooking(id);
            booking.Cancelled = false;

            _context.Update(booking);
            _context.SaveChanges();

            return View("Booking", GetBooking(id));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ManageBooking(Guid id)
        {
            return View(GetBooking(id));
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
            else if (state == (int)BookingFilter.CAN)
            {
                bookings = CancelledBookings();

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

        public List<Booking> CancelledBookings()
        {
            DateTime today = DateTime.Now.Date;
            //Returns a list of the current bookings - i.e. where the customer is staying at the hotel
            List<Booking> bookings = _context.Bookings
                .Include(p => p.Personal).ThenInclude(a => a.Address)
                .Include(r => r.Rooms).ThenInclude(rt => rt.Room.RoomType).Where(c => c.Cancelled == true)
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

        public IActionResult EarningsPartial(int filter)
        {
            FilterOption filterOption = (FilterOption) filter;
            Statistics statistics = new Statistics(GetActiveBookings());
            statistics.Rooms = _context.Rooms.ToList();
            statistics.Filter = filterOption;

            return PartialView("_Earnings", statistics);
        }

        public IActionResult SidepanelPartial()
        {
            return PartialView("_Sidepanel");
        }
    }
}
