using auto_highlighter_iam.DataAccess;
using auto_highlighter_iam.DTOs;
using auto_highlighter_iam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auto_highlighter_iam.Controllers
{
    [ApiController]
    [Route("/api-v1/[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly DataContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IAuthenticationService _authneticationService;
        public AuthenticationController(DataContext db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IAuthenticationService authneticationService)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _authneticationService = authneticationService;
        }

        [HttpPost("[action]")]
        public IActionResult SignIn([FromBody] LoginDTO loginDTO)
        {
            _authneticationService.SignIn(loginDTO.Email, loginDTO.Password);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO registrationDTO)
        {

            IdentityUser newUser = new() { UserName = registrationDTO.UserName, Email = registrationDTO.Email };
            IdentityResult result = await _userManager.CreateAsync(newUser, registrationDTO.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, false);
            }

            return BadRequest();
        }

    }
}
