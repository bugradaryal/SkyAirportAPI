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
    public class PersonalProfile : Profile
    {
        public PersonalProfile()
        {
            CreateMap<PersonalDTO, Personal>();
        }
    }
}
