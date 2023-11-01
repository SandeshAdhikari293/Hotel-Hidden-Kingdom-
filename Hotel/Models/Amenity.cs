
namespace Hotel.Models
{
    public class Amenity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string IconPath { get; set; }

        public ICollection<RoomType>? Rooms { get; set; }
    }
}
