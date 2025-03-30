using AutoMapper;
using DTO;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitys.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<CreateAccountDTO, User>();
        }
    }
}
