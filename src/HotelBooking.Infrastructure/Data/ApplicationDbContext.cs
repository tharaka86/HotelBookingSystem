using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Name).IsRequired().HasMaxLength(200);
                entity.Property(h => h.City).IsRequired().HasMaxLength(100);
                entity.HasIndex(h => h.Name).IsUnique();
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.RoomNumber).IsRequired().HasMaxLength(50);
                entity.Property(r => r.RoomType).HasConversion<int>().IsRequired();
                entity.Property(r => r.Capacity).IsRequired();
                entity.Property(r => r.PricePerNight).HasColumnType("decimal(18,2)");

                entity.HasOne(r => r.Hotel)
                      .WithMany(h => h.Rooms)
                      .HasForeignKey(r => r.HotelId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.BookingReference).IsRequired().HasMaxLength(100);
                entity.Property(b => b.CheckInDate).IsRequired();
                entity.Property(b => b.CheckOutDate).IsRequired();
                entity.Property(b => b.NumberOfGuests).IsRequired();
                entity.Property(b => b.GuestName).IsRequired().HasMaxLength(200);
                entity.Property(b => b.TotalPrice).HasColumnType("decimal(18,2)");
                entity.Property(b => b.BookDate).IsRequired();

                entity.HasOne(b => b.Hotel)
                      .WithMany(h => h.Bookings)
                      .HasForeignKey(b => b.HotelId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Room)
                      .WithMany(r => r.Bookings)
                      .HasForeignKey(b => b.RoomId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.BookingReference).IsUnique();
            });
        }
    }
}
