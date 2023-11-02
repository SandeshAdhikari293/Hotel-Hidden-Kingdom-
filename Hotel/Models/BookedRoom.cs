namespace Hotel.Models
{
    public class BookedRoom
    {
        public Guid Id { get; set; }
        public Room Room { get; set; }
        public bool BreakfastIncluded { get; set; }
        public int Occupants { get; set; }
        
        public Booking Booking { get; set; }

        public double getRoomSubtotal()
        {
            return (Room.RoomType.BasePrice + (Room.RoomType.AdditionalPrice * Occupants)) * Booking.getLengthOfStay();
        }
        public double getBreakfastSubtotal()
        {
            double subtotal = 0;

            if (BreakfastIncluded)
            {
                subtotal = 5.0 * Booking.getLengthOfStay() * Occupants;
            }

            return subtotal;
        }

        public double getRoomsSubtotalPerNight()
        {
            return Room.RoomType.BasePrice + (Room.RoomType.AdditionalPrice * Occupants);
        }

        public double getTotal()
        {
            return getRoomSubtotal() + getBreakfastSubtotal();
        }
    }
}
