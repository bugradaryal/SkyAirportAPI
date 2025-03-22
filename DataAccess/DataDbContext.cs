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
            modelBuilder.Entity<Ticket>().HasKey(x => x.id);
            modelBuilder.Entity<Ticket>().Property(x => x.Price).HasColumnType("DECIMAL(10,2)").IsRequired();
            modelBuilder.Entity<Ticket>().Property(x => x.Puchase_date).HasColumnType("DATETIME").HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Ticket>().Property(x => x.Baggage_weight).HasColumnType("DECIMAL(8,2").HasDefaultValue(0);
            modelBuilder.Entity<Ticket>().Property(x => x.seet_id).IsRequired();
            modelBuilder.Entity<Ticket>().Property(x => x.user_id).IsRequired();
            ////////////////////////////
            modelBuilder.Entity<Seat>().HasKey(x => x.id);
            modelBuilder.Entity<Seat>().Property(x => x.Seat_number).IsRequired();
            modelBuilder.Entity<Seat>().Property(x => x.Seat_Class).HasColumnType("nvarchar").IsRequired().HasMaxLength(32);
            modelBuilder.Entity<Seat>().Property(x => x.Location).HasColumnType("nvarchar").HasDefaultValue("Undefined").HasMaxLength(512);
            modelBuilder.Entity<Seat>().Property(x => x.Is_Available).HasColumnType("boolean").HasDefaultValue(true);
            modelBuilder.Entity<Seat>().Property(x => x.flight_id).IsRequired();
            ////////////////////////////
            modelBuilder.Entity<Personal>().HasKey(x => x.id);
            modelBuilder.Entity<Personal>().Property(x => x.Name).HasColumnType("nvarchar").IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Personal>().Property(x => x.Surname).HasColumnType("nvarchar").IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Personal>().Property(x => x.Role).HasColumnType("nvarchar").HasDefaultValue("Undefined").HasMaxLength(256);
            modelBuilder.Entity<Personal>().Property(x => x.Age).IsRequired();
            modelBuilder.Entity<Personal>().Property(x => x.Gender).HasColumnType("char").HasDefaultValue("U");
            modelBuilder.Entity<Personal>().Property(x => x.PhoneNumber).HasColumnType("nvarchar").HasDefaultValue("Undefined").HasMaxLength(16);
            modelBuilder.Entity<Personal>().Property(x => x.Start_Date).HasColumnType("datetime").HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Personal>().Property(x => x.airport_id).IsRequired();
            ////////////////////////////
            modelBuilder.Entity<OperationalDelay>().HasKey(x => x.id);
            modelBuilder.Entity<OperationalDelay>().Property(x => x.Delay_Reason).HasColumnType("nvarchar").IsRequired().HasMaxLength(1024);
            modelBuilder.Entity<OperationalDelay>().Property(x => x.Delay_Duration).HasColumnType("nvarchar").IsRequired().HasMaxLength(12);
            modelBuilder.Entity<OperationalDelay>().Property(x => x.Delay_Time).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<OperationalDelay>().Property(x => x.flight_id).IsRequired();
            ////////////////////////////
            modelBuilder.Entity<Flight_Aircraft>().HasKey(x => x.id);
            modelBuilder.Entity<Flight_Aircraft>().Property(x => x.flight_id).IsRequired();
            modelBuilder.Entity<Flight_Aircraft>().Property(x => x.aircraft_id).IsRequired();
            ////////////////////////////


















        }
    }
}
