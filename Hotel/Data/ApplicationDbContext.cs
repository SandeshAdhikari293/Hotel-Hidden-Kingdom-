using Hotel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using Image = Hotel.Models.Image;

namespace Hotel.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bed> Beds { get; set; } = default!;
        public DbSet<Amenity> Amenities { get; set; } = default!;
        public DbSet<RoomType> RoomTypes { get; set; } = default!;
        public DbSet<Booking> Bookings { get; set; } = default!;
        public DbSet<Personal> Personals { get; set; } = default!;
        public DbSet<Address> Addresses { get; set; } = default!;
        public DbSet<Room> Rooms { get; set; }
        public DbSet<BookedRoom> BookedRooms { get; set; }
        public DbSet<Image> Images { get; set; }

    }

}