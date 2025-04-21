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
    public class CrewProfile : Profile
    {
        public CrewProfile()
        {
            CreateMap<CrewDTO, Crew>();
        }
    }
}
