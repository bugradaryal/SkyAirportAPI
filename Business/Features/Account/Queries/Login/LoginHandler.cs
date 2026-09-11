using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Features.Account.Queries.GetUserRole;
using DTO;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

using Utilitys.Logging.ExceptionHandler;
using Microsoft.AspNetCore.Http;

namespace Business.Features.Account.Queries.Login
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        public LoginHandler(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            string password = request.loginAccountDTO.Password;
            User? user = await _userManager.FindByEmailAsync(request.loginAccountDTO.Email);
            if (user == null)
                throw new CustomException("User not exist!!", (int)HttpStatusCode.NotFound);
            if (user.IsSuspended)
                throw new CustomException("User's account suspended!", (int)HttpStatusCode.Forbidden);
            var result = await _signInManager.PasswordSignInAsync(user, request.loginAccountDTO.Password, false, lockoutOnFailure: true);
            if (result.Succeeded)
                return new LoginResponse { user = user };
            else if (result.IsLockedOut)
                throw new CustomException("Account is locked. Try again in a few minutes!", (int)HttpStatusCode.Locked);
            else if (result.IsNotAllowed)
                throw new CustomException("Account is not confirmed!", (int)HttpStatusCode.Forbidden);
            else
                throw new CustomException("Invalid username or password.", (int)HttpStatusCode.Unauthorized);
        }
    }
}
