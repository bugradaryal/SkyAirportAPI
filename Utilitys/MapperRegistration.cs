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
            services.AddSingleton<Utilitys.Mapper.IMapper, Utilitys.Mapper.Mapper>();
        }
    }
}
