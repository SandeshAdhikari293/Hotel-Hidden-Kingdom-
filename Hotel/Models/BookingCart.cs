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
        public List<BookedRoom> SelectedRooms { get; set; } = new List<BookedRoom>();
        

        public int getOccupants()
        {
            int occupants = 0;
            foreach(BookedRoom bookedRoom in SelectedRooms)
            {
                occupants = occupants + bookedRoom.Occupants;
            }
            return occupants;
        }

    }
}
