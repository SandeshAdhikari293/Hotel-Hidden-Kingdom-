using Newtonsoft.Json;

namespace Hotel.Models
{
    public class Room
    {

        public Guid Id { get; set; }
        public string Number { get; set; }

        [JsonIgnore]
        public ICollection<Booking> Bookings { get; set; }
        public RoomType? RoomType { get; set;}
    }
}
