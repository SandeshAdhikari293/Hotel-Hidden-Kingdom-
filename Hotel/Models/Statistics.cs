using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Hotel.Models
{
    public class Statistics
    {
        public List<Booking> Bookings { get; set; }
        public List<Room> Rooms { get; set; }
        public FilterOption Filter { get; set; }

        public Statistics(List<Booking> bookings)
        {

            Bookings = bookings;

        }


        public double getEarnings(DateTime date)
        {
            double earnings = 0;
            foreach (Booking booking in Bookings.Where(b => b.BookingDate.Date == date.Date).ToList())
            {
                earnings = earnings + booking.getTotal();
            }
            return earnings;
        }

        public double getEarningsByHour(DateTime date)
        {
            double earnings = 0;
            foreach (Booking booking in Bookings.Where(b => b.BookingDate.Date == date.Date).ToList())
            {
                if(booking.BookingDate.Hour == date.Hour)
                {
                    earnings = earnings + booking.getTotal();
                }

            }
            return earnings;
        }

        public double getEarningsRange(int days)
        {
            int cap = 0;
            if (days == 0)
            {
                cap = 1;
            }
            double earnings = 0;
            for (int i = -days; i < cap; i++)
            {
                earnings = earnings + getEarnings(DateTime.Now.Date.AddDays(i).Date);
            }
            return earnings;

        }

        public string increaseInEarnsFromLastWeek()
        {
            double thisweek = getEarningsRange(7);
            double thisweekAvg = thisweek / 7.0;

            double lastweek = 0;
            for (int i = -14; i < -7; i++)
            {
                lastweek = lastweek + getEarnings(DateTime.Now.Date.AddDays(i).Date);
            }
            if (lastweek == 0) lastweek = 1;

            double lastweekAvg = lastweek / 7.0;

            double result = (((thisweek / lastweek) / lastweek) * 100);


            return result.ToString("0.##");

        }

        public string getLegend(int days)
        {
            if(days < 2)
            {
                return getLegendHours(days);
            }
            string javascriptLegendData = "";
            for (int i = -days; i < 0; i++)
            {
                javascriptLegendData = javascriptLegendData + "," + DateTime.Now.Date.AddDays(i).Date.ToLongDateString();
            }
            return javascriptLegendData;
        }
        public string getEarnings(int days)
        {
            if (days < 2)
            {
                return getEarningHours(days);
            }
            string javascriptEarningsData = "";
            for (int i = -days; i < 0; i++)
            {
                javascriptEarningsData = javascriptEarningsData + "," + getEarnings(DateTime.Now.Date.AddDays(i).Date);
            }
            return javascriptEarningsData;
        }

        public string getLegendHours(int days)
        {
            int cap = 24;
            int start = 0;
            DateTime date = DateTime.Today.AddDays(-1);
            if (days == 0)
            {

                date = DateTime.Today;
            }
            string javascriptLegendData = "";
            for (int i = start; i < cap; i++)
            {
                javascriptLegendData = javascriptLegendData + "," + date.Date.AddHours(i).ToString("hh:00 tt");
            }
            return javascriptLegendData;
        }
        public string getEarningHours(int days)
        {

            int cap = 24;
            int start = 0;
            DateTime date = DateTime.Today.AddDays(-1);
            if (days == 0)
            {

                date = DateTime.Today;
            }
            string javascriptEarningsData = "";
            for (int i = start; i < cap; i++)
            {
                javascriptEarningsData = javascriptEarningsData + "," + getEarningsByHour(date.AddHours(i));
            }
            return javascriptEarningsData;
        }

        public string occupancyRate()
        {
            int roomsTotal = Rooms.Count();
            int rooms = 0;
            foreach(Booking booking in Bookings.Where(cin => cin.CheckIn <= DateTime.Now.Date && cin.CheckOut > DateTime.Now.Date))
            {
                rooms = rooms + booking.Rooms.Count();
            }
            double result = (((double)rooms / (double)roomsTotal) * 100);
            return result.ToString("0.##");

        }
    }
}
