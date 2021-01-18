using auto_highlighter_iam.Constants;
using auto_highlighter_iam.DataAccess;
using auto_highlighter_iam.DTOs;
using auto_highlighter_iam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IAuthenticationService _authenticationService;
        private readonly Services.IAuthorizationService _authorizationService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthenticationController(DataContext db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IAuthenticationService authenticationService, Services.IAuthorizationService authorizationService, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _roleManager = roleManager;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetRoleClaims(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            var claims = await _roleManager.GetClaimsAsync(role);
            return Ok(claims);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignIn([FromBody] LoginDTO loginDTO)
        {
            IdentityUser user = await _authenticationService.SignIn(loginDTO.Email.Trim(), loginDTO.Password);

            return Ok(user);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO registrationDTO)
        {
            if (registrationDTO.Password != registrationDTO.ConfirmPassword) return BadRequest();


            IdentityResult registrationResult = await _authenticationService.Register(registrationDTO.UserName.Trim(), registrationDTO.Email.Trim(), registrationDTO.Password);
            if (!registrationResult.Succeeded) return BadRequest(registrationResult.Errors);

            IdentityUser newUser = await _authenticationService.SignIn(registrationDTO.Email.Trim(), registrationDTO.Password);

            IdentityResult roleResult;

            try
            {
                roleResult = await _authorizationService.AddUserToRole(newUser, Roles.DEFAULT.ToString());
            }
            catch (Exception)
            {
                await _authenticationService.DeleteAccount(registrationDTO.Email.Trim(), registrationDTO.Password);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to initalize user role");
            }

            if (!roleResult.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError, "Failed to initalize user role");


            return CreatedAtAction(nameof(SignIn), new { message = "Successfully created new user" });
        }

    }
}
