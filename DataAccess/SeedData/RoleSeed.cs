using Entities.Enums;
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
    public class RoleSeed : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> entity)
        {
            entity.HasData(
                new IdentityRole { Name = Roles.Administrator.ToString(), NormalizedName = Roles.Administrator.ToString().ToUpper() },
                new IdentityRole { Name = Roles.Support.ToString(), NormalizedName = Roles.Support.ToString().ToUpper() },
                new IdentityRole { Name = Roles.User.ToString(), NormalizedName = Roles.User.ToString().ToUpper() }
            );
        }
    }
}
