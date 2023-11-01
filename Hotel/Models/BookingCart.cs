namespace Hotel.Models
{
    public class BookingCart
    {
        public Booking Booking { get; set; }
        public Guid Id { get; set; }
        public DateTime CheckIn {  get; set; }
        public DateTime CheckOut { get; set; }
        public int People { get; set; }
        public int RoomCount { get; set; }
        public List<Room> SelectedRooms { get; set; } = new List<Room>();

    }
}
