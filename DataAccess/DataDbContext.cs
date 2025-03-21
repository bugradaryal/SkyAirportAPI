using Entities;
using Entities.Moderation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace DataAccess
{
    public class DataDbContext : IdentityDbContext<User>
    {
        private readonly IConfiguration _configuration;

        public DataDbContext(DbContextOptions<DataDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseNpgsql(connectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Personal> Personals { get; set; }
        public DbSet<Flight_Aircraft> Flight_Aircrafts { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Crew_Aircraft> Crew_Aircrafts { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<AdminOperation> AdminOperations { get; set; }
        public DbSet<LogEntry> LogEntrys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().Property(x => x.Name).HasColumnType("nvarchar").IsRequired().HasMaxLength(64);
            modelBuilder.Entity<User>().Property(x => x.Surname).HasColumnType("nvarchar").IsRequired().HasMaxLength(64);
            modelBuilder.Entity<User>().Property(x => x.Gender).HasColumnType("char").HasDefaultValue("U");
            modelBuilder.Entity<User>().Property(x => x.Age).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.IsSuspended).HasColumnType("boolean").HasDefaultValue(false);
            modelBuilder.Entity<User>().Property(x => x.Created_at).HasColumnType("DATETIME").HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<User>().Property(x => x.Uptaded_at).HasColumnType("DATETIME").HasDefaultValue(DateTime.UtcNow);
            ////////////////////////////
            
        }
    }
}
