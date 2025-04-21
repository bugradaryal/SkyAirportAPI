using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Utilitys.Mapper;

namespace Utilitys
{
    public static class MapperRegistration
    {
        public static void AddMapperApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(
                typeof(UserProfile).Assembly,
                typeof(ValidateTokenProfile).Assembly,
                typeof(AirportProfile).Assembly,
                typeof(PersonalProfile).Assembly,
                typeof(AirlineProfile).Assembly,
                typeof(SeatProfile).Assembly,
                typeof(TicketProfile).Assembly,
                typeof(AircraftProfile).Assembly,
                typeof(CrewProfile).Assembly
            );
        }
    }
}
