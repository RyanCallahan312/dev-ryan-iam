﻿
using dev_ryan_iam.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace dev_ryan_iam.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AuthenticationService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> Register(string userName, string email, string password)
        {

            IdentityUser newUser = new() { UserName = userName, Email = email };
            IdentityResult result = await _userManager.CreateAsync(newUser, password);

            return result;
        }

        public async Task<IdentityUser> SignIn(string email, string password)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(email);
            if (user is null) return user;

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
            if (!result.Succeeded) return null;

            return user;
        }

        public async Task<IdentityResult> DeleteAccount(string email, string password)
        {
            IdentityUser user = await SignIn(email, password);
            return await _userManager.DeleteAsync(user);
        }
    }
}
