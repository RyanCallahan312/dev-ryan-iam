using auto_highlighter_iam.Constants;
using auto_highlighter_iam.Extentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace auto_highlighter_iam.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IConfiguration _config;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AuthorizationService(IConfiguration config, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _config = config;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<string> GetToken(IdentityUser user)
        {

            List<Claim> claims = await GetClaims(user);

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));

            string algorithm = SecurityAlgorithms.HmacSha512;

            SigningCredentials signingCredentials = new(key, algorithm);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["JWT:Iss"],
                audience: _config["JWT:Aud"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMilliseconds(_config.GetValue<double>("JWT:Exp")),
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
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
                IEnumerable<string> defaultClaimsTypes = Enum.GetNames(typeof(SuperAdminClaims)).Cast<string>();
                IEnumerable<bool> defaultClaimValues = Enum.GetValues(typeof(SuperAdminClaims)).CastBool();
                await AddClaims(defaultRole, defaultClaimsTypes.Zip(defaultClaimValues, (type, value) => (type, value)));

                IEnumerable<string> adminClaimsTypes = Enum.GetNames(typeof(DefaultClaims)).Cast<string>();
                IEnumerable<bool> adminClaimValues = Enum.GetValues(typeof(DefaultClaims)).CastBool();
                await AddClaims(superAdminRole, adminClaimsTypes.Zip(adminClaimValues, (type, value) => (type, value)));
            }
            catch (Exception)
            {
                await DeleteRole(defaultRole.Name);
                await DeleteRole(superAdminRole.Name);
            }

        }

        private async Task AddClaimValues(IdentityRole role, IEnumerable<string> claimValues)
        {

            foreach (string claimValue in claimValues)
            {
                await _roleManager.AddClaimAsync(role, new Claim(ClaimValueTypes.String, claimValue));
            }
        }

        private async Task AddClaimTypes(IdentityRole role, IEnumerable<string> claimTypes)
        {

            foreach (string claimType in claimTypes)
            {
                await _roleManager.AddClaimAsync(role, new Claim(claimType, true.ToString()));
            }
        }

        private async Task AddClaims<T>(IdentityRole role, IEnumerable<(string, T)> claims)
        {
            foreach ((string claimType, T claimValue) in claims)
            {
                await _roleManager.AddClaimAsync(role, new Claim(claimType, claimValue.ToString()));
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

        private async Task<List<Claim>> GetClaims(IdentityUser user)
        {

            List<Claim> userClaims = new(await _userManager.GetClaimsAsync(user));

            List<string> userRoles = new(await _userManager.GetRolesAsync(user));

            List<Claim> roleClaims = new();
            IdentityRole role;

            foreach (string roleName in userRoles)
            {
                role = await _roleManager.FindByNameAsync(roleName);
                roleClaims = roleClaims.Concat(await _roleManager.GetClaimsAsync(role)).ToList();
                roleClaims.Add(new Claim("roles", roleName));
            }

            List<Claim> allClaims = userClaims.Concat(roleClaims).ToList();

            return allClaims;
        }
    }
}
