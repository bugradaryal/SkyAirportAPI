using Entities;
using Entities.Enums;
using Entities.Moderation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace DataAccess
{
    public class DataDbContext : IdentityDbContext<User>
    {
        public DataDbContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=SkyAirportAPI;Username=postgres;Password=123456;Port=5432");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<OperationalDelay> OperationalDelays { get; set; }
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
        public DbSet<LogLevel> LogLevels { get; set; }
        public DbSet<AircraftStatus> AircraftStatuses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().Property(x => x.Name).HasColumnType("text").IsRequired().HasMaxLength(64);
            modelBuilder.Entity<User>().Property(x => x.Surname).HasColumnType("text").IsRequired().HasMaxLength(64);
            modelBuilder.Entity<User>().Property(x => x.CountryCode).HasColumnType("text").IsRequired().HasMaxLength(5);
            modelBuilder.Entity<User>().Property(x => x.Gender).HasColumnType("char(1)").HasDefaultValue("U");
            modelBuilder.Entity<User>().Property(x => x.Age).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.IsSuspended).HasColumnType("boolean").HasDefaultValue(false);
            modelBuilder.Entity<User>().Property(x => x.Created_at).HasColumnType("TIMESTAMP WITH TIME ZONE").HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<User>().Property(x => x.Uptaded_at).HasColumnType("TIMESTAMP WITH TIME ZONE").HasDefaultValueSql("CURRENT_TIMESTAMP");
            ////////////////////////////
            modelBuilder.Entity<Ticket>().HasKey(x => x.id);
            modelBuilder.Entity<Ticket>().Property(x => x.Price).HasColumnType("DECIMAL(10,2)").IsRequired();
            modelBuilder.Entity<Ticket>().Property(x => x.Puchase_date).HasColumnType("TIMESTAMP WITH TIME ZONE").HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Ticket>().Property(x => x.Baggage_weight).HasColumnType("DECIMAL(8,2)").HasDefaultValue(0);
            modelBuilder.Entity<Ticket>().Property(x => x.seat_id).IsRequired();
            modelBuilder.Entity<Ticket>().Property(x => x.user_id).IsRequired();
            ////////////////////////////
            modelBuilder.Entity<Seat>().HasKey(x => x.id);
            modelBuilder.Entity<Seat>().Property(x => x.Seat_number).IsRequired();
            modelBuilder.Entity<Seat>().Property(x => x.Seat_Class).HasColumnType("text").IsRequired().HasMaxLength(32);
            modelBuilder.Entity<Seat>().Property(x => x.Location).HasColumnType("text").HasDefaultValue("Undefined").HasMaxLength(64);
            modelBuilder.Entity<Seat>().Property(x => x.Is_Available).HasColumnType("boolean").HasDefaultValue(true);
            modelBuilder.Entity<Seat>().Property(x => x.flight_id).IsRequired();
            ////////////////////////////
            modelBuilder.Entity<Personal>().HasKey(x => x.id);
            modelBuilder.Entity<Personal>().Property(x => x.Name).HasColumnType("text").IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Personal>().Property(x => x.Surname).HasColumnType("text").IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Personal>().Property(x => x.Role).HasColumnType("text").IsRequired().HasMaxLength(256);
            modelBuilder.Entity<Personal>().Property(x => x.Age).IsRequired();
            modelBuilder.Entity<Personal>().Property(x => x.Gender).HasColumnType("char(1)").HasDefaultValue("U");
            modelBuilder.Entity<Personal>().Property(x => x.PhoneNumber).HasColumnType("text").HasDefaultValue("Undefined").HasMaxLength(16);
            modelBuilder.Entity<Personal>().HasIndex(x => x.PhoneNumber).IsUnique();
            modelBuilder.Entity<Personal>().Property(x => x.Start_Date).HasColumnType("TIMESTAMP WITH TIME ZONE").HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Personal>().Property(x => x.airport_id).IsRequired();
            ////////////////////////////
            modelBuilder.Entity<OperationalDelay>().HasKey(x => x.id);
            modelBuilder.Entity<OperationalDelay>().Property(x => x.Delay_Reason).HasColumnType("text").IsRequired().HasMaxLength(1024);
            modelBuilder.Entity<OperationalDelay>().Property(x => x.Delay_Duration).HasColumnType("text").IsRequired().HasMaxLength(12);
            modelBuilder.Entity<OperationalDelay>().Property(x => x.Delay_Time).HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
            modelBuilder.Entity<OperationalDelay>().Property(x => x.flight_id).IsRequired();
            ////////////////////////////
            modelBuilder.Entity<Flight_Aircraft>().HasKey(x => x.id);
            modelBuilder.Entity<Flight_Aircraft>().Property(x => x.flight_id).IsRequired();
            modelBuilder.Entity<Flight_Aircraft>().Property(x => x.aircraft_id).IsRequired();
            ////////////////////////////
            modelBuilder.Entity<Flight>().HasKey(x => x.id);
            modelBuilder.Entity<Flight>().Property(x => x.Name).HasColumnType("text").IsRequired().HasMaxLength(128);
            modelBuilder.Entity<Flight>().Property(x => x.Description).HasColumnType("text").HasDefaultValue("No Description").HasMaxLength(1024);
            modelBuilder.Entity<Flight>().Property(x => x.Arrival_Date).HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
            modelBuilder.Entity<Flight>().Property(x => x.Deperture_Date).HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
            modelBuilder.Entity<Flight>().Property(x => x.Status).HasColumnType("text").IsRequired().HasMaxLength(32);
            modelBuilder.Entity<Flight>().Property(x => x.airline_id).IsRequired(); ;
            ////////////////////////////
            modelBuilder.Entity<Crew_Aircraft>().HasKey(x => x.id);
            modelBuilder.Entity<Crew_Aircraft>().Property(x => x.crew_id).IsRequired();
            modelBuilder.Entity<Crew_Aircraft>().Property(x => x.aircraft_id).IsRequired();
            ////////////////////////////
            modelBuilder.Entity<Crew>().HasKey(x => x.id);
            modelBuilder.Entity<Crew>().Property(x => x.Name).HasColumnType("text").IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Crew>().Property(x => x.Surname).HasColumnType("text").IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Crew>().Property(x => x.Role).HasColumnType("text").IsRequired().HasMaxLength(256);
            modelBuilder.Entity<Crew>().Property(x => x.Age).IsRequired();
            modelBuilder.Entity<Crew>().Property(x => x.Gender).HasColumnType("char(1)").HasDefaultValue("U");
            ////////////////////////////
            modelBuilder.Entity<Airport>().HasKey(x => x.id);
            modelBuilder.Entity<Airport>().Property(x => x.Name).HasColumnType("text").IsRequired().HasMaxLength(128);
            modelBuilder.Entity<Airport>().Property(x => x.Location).HasColumnType("text").IsRequired().HasMaxLength(512);
            modelBuilder.Entity<Airport>().Property(x => x.PhoneNumber).HasColumnType("text").IsRequired().HasMaxLength(16);
            modelBuilder.Entity<Airport>().HasIndex(x => x.PhoneNumber).IsUnique();
            modelBuilder.Entity<Airport>().Property(x => x.MailAdress).HasColumnType("text").HasDefaultValue("Undefined").HasMaxLength(96);
            modelBuilder.Entity<Airport>().HasIndex(x => x.MailAdress).IsUnique();
            modelBuilder.Entity<Airport>().Property(x => x.Description).HasColumnType("text").HasDefaultValue("No Description").HasMaxLength(1024);
            modelBuilder.Entity<Airport>().Property(x => x.Status).HasColumnType("text").IsRequired().HasMaxLength(32);
            ////////////////////////////
            modelBuilder.Entity<Airline>().HasKey(x => x.id);
            modelBuilder.Entity<Airline>().Property(x => x.Name).HasColumnType("text").IsRequired().HasMaxLength(128);
            modelBuilder.Entity<Airline>().Property(x => x.Description).HasColumnType("text").HasDefaultValue("No Description").HasMaxLength(1024);
            modelBuilder.Entity<Airline>().Property(x => x.WebAdress).HasColumnType("text").IsRequired().HasMaxLength(128);
            modelBuilder.Entity<Airline>().Property(x => x.PhoneNumber).HasColumnType("text").IsRequired().HasMaxLength(16);
            modelBuilder.Entity<Airline>().HasIndex(x => x.PhoneNumber).IsUnique();
            modelBuilder.Entity<Airline>().Property(x => x.Country).HasColumnType("text").IsRequired().HasMaxLength(60);
            modelBuilder.Entity<Airline>().Property(x => x.airport_id).IsRequired();
            ////////////////////////////
            modelBuilder.Entity<Aircraft>().HasKey(x => x.id);
            modelBuilder.Entity<Aircraft>().Property(x => x.Model).HasColumnType("text").IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Aircraft>().Property(x => x.Last_Maintenance_Date).HasColumnType("TIMESTAMP WITH TIME ZONE");
            modelBuilder.Entity<Aircraft>().Property(x => x.Fuel_Capacity).HasColumnType("DECIMAL(7,1)").IsRequired();
            modelBuilder.Entity<Aircraft>().Property(x => x.Max_Altitude).HasColumnType("DECIMAL(8,1)").IsRequired();
            modelBuilder.Entity<Aircraft>().Property(x => x.Engine_Power).IsRequired();
            modelBuilder.Entity<Aircraft>().Property(x => x.Carry_Capacity).HasColumnType("DECIMAL(7,2)").IsRequired();
            modelBuilder.Entity<Aircraft>().Property(x => x.aircraftStatus_id).IsRequired();
            modelBuilder.Entity<Aircraft>().Property(x => x.Current_Capacity).HasColumnType("DECIMAL(7,2)").HasDefaultValue(0);
            ////////////////////////////
            modelBuilder.Entity<LogEntry>().HasKey(x => x.id);
            modelBuilder.Entity<LogEntry>().Property(x => x.Timestamp).HasColumnType("TIMESTAMP WITH TIME ZONE").HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<LogEntry>().Property(x => x.Message).HasColumnType("text").IsRequired().HasMaxLength(5096);
            modelBuilder.Entity<LogEntry>().Property(x => x.Action_type).HasConversion(new EnumToStringConverter<Action_Type>()).IsRequired();
            modelBuilder.Entity<LogEntry>().Property(x => x.Target_table).HasColumnType("text").HasMaxLength(64);
            modelBuilder.Entity<LogEntry>().Property(x => x.Record_id).IsRequired();
            modelBuilder.Entity<LogEntry>().Property(e => e.AdditionalData)
                .HasConversion(
                        v => JsonConvert.SerializeObject(v),  // List<string> → JSON string
                        v => JsonConvert.DeserializeObject<List<string>>(v) ?? new List<string>()) // JSON string → List<string>
                .HasColumnType("jsonb"); // PostgreSQL JSONB tipi
            modelBuilder.Entity<LogEntry>().Property(x => x.user_id).IsRequired();
            modelBuilder.Entity<LogEntry>().Property(x => x.loglevel_id).IsRequired();
            ////////////////////////////
            modelBuilder.Entity<AdminOperation>().HasKey(x => x.id);
            modelBuilder.Entity<AdminOperation>().Property(x => x.Operation_type).HasConversion(new EnumToStringConverter<Operation_Type>()).IsRequired();
            modelBuilder.Entity<AdminOperation>().Property(x => x.Target_table).HasColumnType("text").IsRequired().HasMaxLength(64);
            modelBuilder.Entity<AdminOperation>().Property(x => x.Target_id).IsRequired();
            modelBuilder.Entity<AdminOperation>().Property(x => x.Operation_Date).HasColumnType("TIMESTAMP WITH TIME ZONE").HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<AdminOperation>().Property(x => x.user_id).IsRequired();
            ////////////////////////////
            modelBuilder.Entity<LogLevel>().HasKey(x => x.id);
            modelBuilder.Entity<LogLevel>().Property(x => x.Level).HasColumnType("text").IsRequired().HasMaxLength(64);
            modelBuilder.Entity<LogLevel>().HasIndex(x => x.Level).IsUnique();
            ////////////////////////////
            modelBuilder.Entity<AircraftStatus>().HasKey(x => x.id);
            modelBuilder.Entity<AircraftStatus>().Property(x => x.Status).HasColumnType("text").HasMaxLength(64).HasDefaultValue("NotOperational");
            modelBuilder.Entity<AircraftStatus>().HasIndex(x => x.Status).IsUnique();
            ////////////////////////////
            modelBuilder.Entity<Ticket>().HasOne<User>(s => s.user).WithMany(g => g.ticket).HasForeignKey(s => s.user_id).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AdminOperation>().HasOne<User>(s => s.user).WithMany(g => g.adminOperation).HasForeignKey(s => s.user_id).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<LogEntry>().HasOne<User>(s => s.user).WithMany(g => g.logEntry).HasForeignKey(s => s.user_id).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Seat>().HasOne<Ticket>(s => s.ticket).WithOne(g => g.seat).HasForeignKey<Ticket>(s => s.seat_id).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Seat>().HasOne<Flight>(s => s.flight).WithMany(g => g.seat).HasForeignKey(s => s.flight_id).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Flight_Aircraft>().HasOne<Flight>(s => s.flight).WithMany(g => g.flight_Aircraft).HasForeignKey(s => s.flight_id).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Flight_Aircraft>().HasOne<Aircraft>(s => s.aircraft).WithMany(g => g.flight_Aircraft).HasForeignKey(s => s.aircraft_id).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Flight>().HasOne<OperationalDelay>(s => s.operationalDelay).WithOne(g => g.flight).HasForeignKey<OperationalDelay>(s => s.flight_id).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Crew_Aircraft>().HasOne<Crew>(s => s.crew).WithMany(g => g.crew_Aircraft).HasForeignKey(s => s.crew_id).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Crew_Aircraft>().HasOne<Aircraft>(s => s.aircraft).WithMany(g => g.crew_Aircraft).HasForeignKey(s => s.aircraft_id).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Flight>().HasOne<Airline>(s => s.airline).WithMany(g => g.flight).HasForeignKey(s => s.airline_id).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Airline>().HasOne<Airport>(s => s.airport).WithMany(g => g.airline).HasForeignKey(s => s.airport_id).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Personal>().HasOne<Airport>(s => s.airport).WithMany(g => g.personal).HasForeignKey(s => s.airport_id).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<LogEntry>().HasOne<LogLevel>(s => s.logLevel).WithMany(g => g.logEntry).HasForeignKey(s => s.loglevel_id).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Aircraft>().HasOne<AircraftStatus>(s => s.aircraftStatus).WithMany(g => g.aircraft).HasForeignKey(s => s.aircraftStatus_id).OnDelete(DeleteBehavior.NoAction);
            ////////////////////////////
            modelBuilder.ApplyConfiguration(new SeedData.LevelSeed());
            modelBuilder.ApplyConfiguration(new SeedData.RoleSeed());
            modelBuilder.ApplyConfiguration(new SeedData.StatusSeed());
        }
    }
}
