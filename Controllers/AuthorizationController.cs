using auto_highlighter_iam.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using auto_highlighter_iam.Services;
using Microsoft.AspNetCore.Http;

namespace auto_highlighter_iam.Controllers
{
    [ApiController]
    [Route("/api-v1/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Services.IAuthorizationService _authorizationService;

        public AuthorizationController(ILogger<AuthorizationController> logger, IConfiguration config, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, Services.IAuthorizationService authorizationService)
        {
            _logger = logger;
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Authorize(Roles = "SUPERADMIN")]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Default")]
        public async Task<IActionResult> CreateSuperAdmin()
        {

            IdentityUser superAdminUser = await _userManager.FindByEmailAsync(_config["SuperAdmin:Email"]);

            IdentityResult result = await _authorizationService.AddUserToRole(superAdminUser, Roles.SUPERADMIN.ToString());

            if (!result.Succeeded) return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return Ok();
        }
    }
}
