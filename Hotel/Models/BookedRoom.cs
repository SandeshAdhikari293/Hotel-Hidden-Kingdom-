namespace Hotel.Models
{
    public class BookedRoom
    {
        public Guid Id { get; set; }
        public Room Room { get; set; }
        public bool BreakfastIncluded { get; set; }
        public int Occupants { get; set; }
    }
}
