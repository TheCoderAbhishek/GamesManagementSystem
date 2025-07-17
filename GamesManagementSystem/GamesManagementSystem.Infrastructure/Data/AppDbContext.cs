using GamesManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamesManagementSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<GameDevice> GameDevices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the composite primary key for our join table
            modelBuilder.Entity<GameDevice>()
                .HasKey(gd => new { gd.GameId, gd.DeviceId });

            // Configure the many-to-many relationship from Game to Device
            modelBuilder.Entity<GameDevice>()
                .HasOne(gd => gd.Game)
                .WithMany(g => g.GameDevices)
                .HasForeignKey(gd => gd.GameId);

            // Configure the many-to-many relationship from Device to Game
            modelBuilder.Entity<GameDevice>()
                .HasOne(gd => gd.Device)
                .WithMany(d => d.GameDevices)
                .HasForeignKey(gd => gd.DeviceId);
        }
    }
}
