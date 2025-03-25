using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SeedData
{
    public class StatusSeed : IEntityTypeConfiguration<AircraftStatus>
    {
        public void Configure(EntityTypeBuilder<AircraftStatus> entity)
        {
            entity.HasData(
                new AircraftStatus { id = 1, Status = "Available"},
                new AircraftStatus { id = 2, Status = "InMaintenance"},
                new AircraftStatus { id = 3, Status = "OutOfService"},
                new AircraftStatus { id = 4, Status = "NotOperational"}
            );
        }
    }
}
