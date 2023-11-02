namespace Hotel.Models
{
    public class Booking
    {
        public Guid Id { get; set; }
        public DateTime BookingDate {  get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int People { get; set; }
        public int RoomCount { get; set; }
        public double Price { get; set; }
        public ICollection<BookedRoom> Rooms { get; set; }
        public Personal? Personal { get; set; }
        public string? PaymentTransactionId { get; set; }
        public bool Cancelled {  get; set; }
        public string? AdditionalInformation { get; set; }

        public Booking()
        {
            Rooms = new List<BookedRoom>();
        }

        public int getLengthOfStay()
        {
            return (CheckOut - CheckIn).Days;
        }

        public double calculatePrice()
        {
            int nights = getLengthOfStay();
            double price = 0;

            foreach (var room in Rooms)
            {
                price = price + (room.Room.RoomType.BasePrice * nights);
            }

            return price;
        }

        public double getRoomSubtotal()
        {
            double subtotal = 0;
            foreach(BookedRoom bookedRoom in Rooms)
            {
                subtotal = subtotal + bookedRoom.getRoomSubtotal();
            }
            return subtotal;
        }

        public double getTotal()
        {
            return getRoomSubtotal() + getBreakfastSubtotal();
        }

        public double getBreakfastSubtotal()
        {
            double subtotal = 0;
            foreach (BookedRoom bookedRoom in Rooms)
            {
                subtotal = subtotal + bookedRoom.getBreakfastSubtotal();
            }
            return subtotal;
        }
    }
}
