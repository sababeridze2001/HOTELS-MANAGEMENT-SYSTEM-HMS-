using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hotel.Models.Entities;

namespace Hotel.Repository
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

        public DbSet<Hotel.Models.Entities.Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<GuestReservation> GuestReservations { get; set; }
        public DbSet<User> Users { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<GuestReservation>()
                .HasKey(gr => new { gr.GuestId, gr.ReservationId });

            
            modelBuilder.Entity<Manager>()
                .HasIndex(m => m.Email)
                .IsUnique();

            modelBuilder.Entity<Manager>()
                .HasIndex(m => m.PersonalNumber)
                .IsUnique();

            
            modelBuilder.Entity<Hotel.Models.Entities.Hotel>()
                .HasOne(h => h.Manager)
                .WithMany()
                .HasForeignKey(h => h.ManagerId)
                .OnDelete(DeleteBehavior.Restrict); 

            

            
            modelBuilder.Entity<Manager>().HasData(
                new Manager
                {
                    Id = 1,
                    Name = "Saba",
                    Surname = "Beridze",
                    PersonalNumber = "12345678901",
                    Email = "saba@example.com",
                    MobileNumber = "+995599000000"
                }
            );

            
            modelBuilder.Entity<Hotel.Models.Entities.Hotel>().HasData(
                new Hotel.Models.Entities.Hotel
                {
                    Id = 1,
                    Name = "Grand Hotel",
                    Rating = 5,
                    Country = "Georgia",
                    City = "Tbilisi",
                    Address = "Rustaveli Avenue 10",
                    ManagerId = 1
                }
            );

            
            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, Name = "Deluxe Room", IsAvailable = true, Price = 150, HotelId = 1 },
                new Room { Id = 2, Name = "Standard Room", IsAvailable = true, Price = 100, HotelId = 1 }
            );

            
            modelBuilder.Entity<Guest>().HasData(
                new Guest { Id = 1, Name = "John", Surname = "Doe", PersonalNumber = "98765432101", MobileNumber = "+995599111111" },
                new Guest { Id = 2, Name = "Alice", Surname = "Smith", PersonalNumber = "87654321098", MobileNumber = "+995599222222" }
            );

            
            modelBuilder.Entity<Reservation>().HasData(
                new Reservation
                {
                    Id = 1,
                    CheckInDate = new DateTime(2024, 3, 10, 14, 0, 0),
                    CheckOutDate = new DateTime(2024, 3, 15, 11, 0, 0),
                    RoomId = 1
                }
            );

            
            modelBuilder.Entity<GuestReservation>().HasData(
                new GuestReservation { GuestId = 1, ReservationId = 1 }
            );

            
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "password123", 
                    Role = "Admin"
                },
                new User
                {
                    Id = 10,
                    Username = "saba_manager",
                    Password = "manager123", 
                    Role = "Manager"
                },
                new User
                {
                    Id = 11,
                    Username = "john_guest",
                    Password = "guest123", 
                    Role = "Guest"
                }
            );

            base.OnModelCreating(modelBuilder);
        }

    }
}