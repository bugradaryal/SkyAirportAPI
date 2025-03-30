using AutoMapper;
using AutoMapper.Internal;
using Business.Abstract;
using Business.ExceptionHandler;
using DTO;
using Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AccountManager : IAccountServices
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signManager;
        private readonly IMapper _mapper;
        public AccountManager(UserManager<User> userManager, IMapper mapper, SignInManager<User> signManager) 
        {
            _mapper = mapper;
            _userManager = userManager;
            _signManager = signManager;
        }

        public async Task CreateAccount(CreateAccountDTO createAccountDTO)
        {
            try
            {
                User user = _mapper.Map<User>(createAccountDTO);
                string password = createAccountDTO.Password;
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                    throw new CustomException(result.Errors.FirstOrDefault().ToString(), (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex) 
            {
                throw new CustomException(ex.Message, (int)HttpStatusCode.BadRequest);
            }
        }
        public async Task LoginAccount(LoginAccountDTO loginAccountDTO)
        {
            try 
            { 
                string password = loginAccountDTO.Password;
                var result = await _signManager.PasswordSignInAsync(loginAccountDTO.Email, loginAccountDTO.Password,false,lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _signManager.ref
                }
            }
            catch (Exception ex) 
            {
                throw new CustomException(ex.Message, (int) HttpStatusCode.BadRequest);
            }
        }
    }
}
