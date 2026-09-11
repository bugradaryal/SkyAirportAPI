using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.SeedData
{
    public class StatusSeed : IEntityTypeConfiguration<AircraftStatus>
    {
        public void Configure(EntityTypeBuilder<AircraftStatus> entity)
        {
            entity.HasData(
                new AircraftStatus { id = 1, Status = "Available" },
                new AircraftStatus { id = 2, Status = "InMaintenance" },
                new AircraftStatus { id = 3, Status = "OutOfService" },
                new AircraftStatus { id = 4, Status = "NotOperational" }
            );
        }
    }
}
