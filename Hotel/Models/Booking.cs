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
        public ICollection<Room> Rooms { get; set; }
        public Personal? Personal { get; set; }
        public string? PaymentTransactionId { get; set; }

        public Booking()
        {
            Rooms = new List<Room>();
        }

        public int getLengthOfStay()
        {
            return (CheckOut - CheckIn).Days;
        }

        public double calculatePrice()
        {
            int nights = getLengthOfStay();
            double price = 0;

            foreach (Room room in Rooms)
            {
                price = price + (room.RoomType.BasePrice * nights);
            }

            return price;
        }
    }
}
