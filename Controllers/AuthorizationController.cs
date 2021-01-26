using dev_ryan_iam.Constants;
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
using dev_ryan_iam.Services;
using Microsoft.AspNetCore.Http;

namespace dev_ryan_iam.Controllers
{
    [ApiController]
    [Route("/api-v1/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly Services.IAuthorizationService _authorizationService;

        public AuthorizationController(IConfiguration config, UserManager<IdentityUser> userManager, Services.IAuthorizationService authorizationService)
        {
            _config = config;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "SUPERADMIN")]
        public IActionResult TestSuperAdmin()
        {
            return Ok();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "DEFAULT")]
        public IActionResult TestDefault()
        {
            return Ok();
        }

        [HttpGet("[action]")]
        [Authorize]
        public IActionResult TestAny()
        {
            return Ok();
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "DEFAULT")]
        public async Task<IActionResult> CreateSuperAdmin()
        {

            IdentityUser superAdminUser = await _userManager.FindByEmailAsync(_config["SuperAdmin:Email"]);

            IdentityResult result = await _authorizationService.AddUserToRole(superAdminUser, Roles.SUPERADMIN.ToString());

            if (!result.Succeeded) return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return Ok();
        }
    }
}
