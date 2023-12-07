using API_JiggysCarRental.MODELS;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API_JiggysCarRental.DATA
{
    public class ApplicationDbContext:IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

       
        public DbSet<Customer> Customers { get; set; }               
        public DbSet<Parish> Parishes { get; set; }


        public DbSet<VehicleCategory> VehicleCategories { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }                
        public DbSet<Availability> Availabilites { get; set; }      


        public DbSet<AddOn> AddOns { get; set; }
        public DbSet<Booking> Bookings { get; set; }                
        public DbSet<BookingAddOn> BookingAddOns { get; set; }      


    }
}
