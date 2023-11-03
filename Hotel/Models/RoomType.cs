using Newtonsoft.Json;

namespace Hotel.Models
{
    public class RoomType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double BasePrice { get; set; }
        public double AdditionalPrice { get; set; }
        public List<Bed>? Beds { get; set; }
        public List<Amenity>? Amenities { get; set; }

        public RoomType()
        {
            Beds = new List<Bed>();
            Amenities = new List<Amenity>();
        }

        public int maxPeople()
        {
            int count = 0;
            foreach (Bed bed in Beds)
            {
                count = count + bed.MaxPeople;
            }
            return count;
        }

        public double getPrice(int people)
        {
            double price = BasePrice + (AdditionalPrice * people);


            return price;
        }
    }
}
