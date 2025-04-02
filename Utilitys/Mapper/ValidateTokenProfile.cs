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
    public class ValidateTokenProfile : Profile
    {
        public ValidateTokenProfile()
        {
            CreateMap<ValidateTokenDTO, AuthenticationModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.user.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.user.Email))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.roles))
                .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token));
        }
    }
}
