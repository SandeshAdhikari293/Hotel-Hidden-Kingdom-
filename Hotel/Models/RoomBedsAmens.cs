namespace Hotel.Models
{
    public class RoomBedsAmens
    {
        public RoomType Room { get; set; }
        public List<Bed> Beds { get; set; }
        public List<Amenity> Amenities { get; set; }
    }
}
