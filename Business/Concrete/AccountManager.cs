
using AutoMapper;
using AutoMapper.Internal;
using Business.Abstract;
using Business.ExceptionHandler;
using DTO;
using Entities;
using Entities.Configuration;
using Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                else
                {
                    var roleResult = await _userManager.AddToRoleAsync(user,Default_Authorization_Type.default_role.ToString());
                    if (!roleResult.Succeeded)
                        throw new CustomException(roleResult.Errors.FirstOrDefault().ToString(), (int)HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex) 
            {
                throw new CustomException(ex.Message, (int)HttpStatusCode.BadRequest);
            }
        }
        public async Task<User> LoginAccount(LoginAccountDTO loginAccountDTO)
        {
            try 
            { 
                string password = loginAccountDTO.Password;
                var result = await _signManager.PasswordSignInAsync(loginAccountDTO.Email, loginAccountDTO.Password,false,lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    User user = await _userManager.FindByEmailAsync(loginAccountDTO.Email);
                    return user;
                }
                else if (result.IsLockedOut)
                    throw new CustomException("Account is locked. Try few minutes later!", (int)HttpStatusCode.BadRequest);
                else if(result.IsNotAllowed)
                    throw new CustomException("Account in not confirmed!", (int)HttpStatusCode.BadRequest);
                else
                    throw new CustomException("Invalid username or password.", (int)HttpStatusCode.BadRequest);

            }
            catch (Exception ex) 
            {
                throw new CustomException(ex.Message, (int) HttpStatusCode.BadRequest);
            }
        }
        public async Task<List<string>> GetUserRoles(string userId)
        {
            try
            {
                var roles = await _userManager.GetRolesAsync(new User { Id = userId });
                return roles.ToList();  // İstenirse ToList() ile dönüştürülür, fakat IList de çalışır
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, (int)HttpStatusCode.BadRequest);
            }
        }
        public async Task DeleteAccount(User user)
        {
            try
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    throw new CustomException(result.Errors.FirstOrDefault().ToString(), (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, (int)HttpStatusCode.BadRequest);
            }

        }
        public async Task UpdateAccount(UpdateAccountDTO updateAccountDTO)
        {
            try
            {
                var result = await _userManager.UpdateAsync(_mapper.Map<User>(updateAccountDTO));///////////////
                if (!result.Succeeded)
                    throw new CustomException(result.Errors.FirstOrDefault().ToString(), (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
