using Entities;
using Entities.Moderation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SeedData
{
    public class LevelSeed : IEntityTypeConfiguration<LogLevel>
    {
        public void Configure(EntityTypeBuilder<LogLevel> entity)
        {
            entity.HasData(
                new LogLevel { id = 1, Level = "Trace" },
                new LogLevel { id = 2, Level = "Debug" },
                new LogLevel { id = 3, Level = "Info" },
                new LogLevel { id = 4, Level = "Warn" },
                new LogLevel { id = 5, Level = "Error" },
                new LogLevel { id = 6, Level = "Fatal" },
                new LogLevel { id = 7, Level = "Alert" },
                new LogLevel { id = 8, Level = "Emergency" }
            );
        }
    }
}
