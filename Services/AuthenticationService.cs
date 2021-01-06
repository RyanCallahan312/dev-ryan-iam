using auto_highlighter_iam.DataAccess;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auto_highlighter_iam.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly DataContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AuthenticationService(DataContext db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _db = db;
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
            if(user is null) return user;

            SignInResult result = await _signInManager.PasswordSignInAsync(user, password, false, true);
            if (!result.Succeeded) return null;

            return user;
        }
    }
}
