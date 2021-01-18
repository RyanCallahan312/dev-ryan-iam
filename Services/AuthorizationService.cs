using auto_highlighter_iam.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace auto_highlighter_iam.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AuthorizationService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddUserToRole(IdentityUser user, string role)
        {
            bool doesRoleExist = await _roleManager.RoleExistsAsync(role);


            if (!doesRoleExist && (role == Roles.SUPERADMIN.ToString() || role == Roles.DEFAULT.ToString()))
            {
                await CreateInitalRoles();
            }

            return await _userManager.AddToRoleAsync(user, role);
        }

        private async Task CreateInitalRoles()
        {
            IdentityRole defaultRole = await CreateRole(Roles.DEFAULT.ToString());
            IdentityRole superAdminRole = await CreateRole(Roles.SUPERADMIN.ToString());

            try
            {
                await AddClaims(defaultRole, Enum.GetNames(typeof(DefaultClaims)).Cast<string>());
                await AddClaims(superAdminRole, Enum.GetNames(typeof(SuperAdminClaims)).Cast<string>());
            }
            catch (Exception)
            {
                await DeleteRole(defaultRole.Name);
                await DeleteRole(superAdminRole.Name);
            }

        }

        private async Task AddClaims(IdentityRole role, IEnumerable<string> claimValues)
        {

            foreach (string claimValue in claimValues)
            {
                await _roleManager.AddClaimAsync(role, new Claim(ClaimValueTypes.String, claimValue));
            }
        }

        private async Task<IdentityRole> CreateRole(string name)
        {
            IdentityRole role = await _roleManager.FindByNameAsync(name);

            if (role is null)
            {
                role = new(name);
                await _roleManager.CreateAsync(role);
            }

            return role;
        }

        private async Task<IdentityResult> DeleteRole(string name)
        {
            IdentityRole role = await _roleManager.FindByNameAsync(name);

            return await _roleManager.DeleteAsync(role);
        }
    }
}
