using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DTO;
using Entities;

namespace Utilitys.Mapper
{
    public class AirportProfile : Profile
    {
        public AirportProfile() 
        {
            CreateMap<AirportDTO, Airport>();
        }
    }
}
