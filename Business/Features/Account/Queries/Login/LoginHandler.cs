using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Business.Features.Account.Queries.GetUserRole;
using DTO;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Utilitys.ResponseHandler;

namespace Business.Features.Account.Queries.Login
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        public LoginHandler(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                string password = request.loginAccountDTO.Password;
                User user = await _userManager.FindByEmailAsync(request.loginAccountDTO.Email);
                if (user == null)
                    return new LoginResponse { response = new ResponseModel { Message = "User not exist!" } };
                if (user.IsSuspended)
                    return new LoginResponse { response = new ResponseModel { Message = "User's account suspended!"} };
                var result = await _signInManager.PasswordSignInAsync(user, request.loginAccountDTO.Password, false, lockoutOnFailure: true);
                if (result.Succeeded)
                    return new LoginResponse { user = user, error = false };
                else if (result.IsLockedOut)
                    return new LoginResponse { response = new ResponseModel { Message = "Account is locked. Try few minutes later" } };
                else if (result.IsNotAllowed)
                    return new LoginResponse { response = new ResponseModel { Message = "Account in not confirmed!" } };
                else
                    return new LoginResponse { response = new ResponseModel { Message = "Invalid username or password." } };

            }
            catch (Exception ex)
            {
                return new LoginResponse { response = new ResponseModel { Message = "Exception Throw!", Exception = new CustomException(ex.Message, 4, (int)HttpStatusCode.BadRequest) } };
            }
        }
    }
}
