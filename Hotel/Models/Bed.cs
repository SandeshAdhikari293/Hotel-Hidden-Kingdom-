using Newtonsoft.Json;

namespace Hotel.Models
{
    public class Bed
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxPeople { get; set; }

        [JsonIgnore]
        public ICollection<RoomType>? Rooms { get; set; }

    }
}
