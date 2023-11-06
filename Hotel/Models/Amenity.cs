
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class Amenity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? IconPath { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public IFormFile? Icon { get; set; }

        public ICollection<RoomType>? Rooms { get; set; }
    }
}
