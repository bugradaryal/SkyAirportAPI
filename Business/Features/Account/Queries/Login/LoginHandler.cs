using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.ExceptionHandler;
using Business.Features.Account.Queries.GetUserRole;
using DTO;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

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
                    return new LoginResponse { exception = new CustomException("User not exist!", (int)HttpStatusCode.NotFound) };
                if (user.IsSuspended)
                    return new LoginResponse { exception = new CustomException("User's account suspended!", (int)HttpStatusCode.Unauthorized) };
                var result = await _signInManager.PasswordSignInAsync(user, request.loginAccountDTO.Password, false, lockoutOnFailure: true);
                if (result.Succeeded)
                    return new LoginResponse { user = user, error = false };
                else if (result.IsLockedOut)
                    return new LoginResponse { exception = new CustomException("Account is locked. Try few minutes later!", (int)HttpStatusCode.BadRequest) };
                else if (result.IsNotAllowed)
                    return new LoginResponse { exception = new CustomException("Account in not confirmed!", (int)HttpStatusCode.BadRequest) };
                else
                    return new LoginResponse { exception = new CustomException("Invalid username or password.", (int)HttpStatusCode.BadRequest) };

            }
            catch (Exception ex)
            {
                return new LoginResponse { exception = new CustomException(ex.Message, (int)HttpStatusCode.BadRequest) };
            }
        }
    }
}
