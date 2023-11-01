namespace Hotel.Models
{
    public class BookingRooms
    {
        public BookingCart BookingCart { get; set; }
        public Dictionary<RoomType, List<Room>> RoomList { get; set;}

    }
}
