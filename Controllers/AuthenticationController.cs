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
        public async Task<IActionResult> SignIn([FromBody] LoginDTO loginDTO)
        {
            IdentityUser user = await _authneticationService.SignIn(loginDTO.Email, loginDTO.Password);

            return Ok(user);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO registrationDTO)
        {
            if (registrationDTO.Password != registrationDTO.ConfirmPassword) return BadRequest();

            IdentityResult registrationResult = await _authneticationService.Register(registrationDTO.UserName, registrationDTO.Email, registrationDTO.Password);

            if (!registrationResult.Succeeded) return BadRequest();

            return CreatedAtAction(nameof(SignIn), new { message = "Successfully created new user" });
        }

    }
}
