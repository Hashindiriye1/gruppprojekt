using CarWashBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarWashBooking.Infrastructure;

public class CarWashDbContext : DbContext
{
    public CarWashDbContext(DbContextOptions<CarWashDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Domain.Service> Services => Set<Domain.Service>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(200);
            e.Property(x => x.Email).HasMaxLength(256);
            e.Property(x => x.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<Vehicle>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Customer).WithMany(c => c.Vehicles).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.LicensePlate).HasMaxLength(20);
            e.Property(x => x.Make).HasMaxLength(100);
            e.Property(x => x.Model).HasMaxLength(100);
            e.HasIndex(x => x.CustomerId);
        });

        modelBuilder.Entity<Domain.Service>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(1000);
            e.Property(x => x.Price).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Location>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(200);
            e.Property(x => x.Address).HasMaxLength(500);
        });

        modelBuilder.Entity<Booking>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Customer).WithMany(c => c.Bookings).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Vehicle).WithMany(v => v.Bookings).HasForeignKey(x => x.VehicleId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Service).WithMany(s => s.Bookings).HasForeignKey(x => x.ServiceId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Location).WithMany(l => l.Bookings).HasForeignKey(x => x.LocationId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(x => x.ScheduledDate);
            e.HasIndex(x => x.Status);
            e.HasIndex(x => new { x.LocationId, x.ScheduledDate });
        });
    }
}
