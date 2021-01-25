using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace auto_highlighter_iam.Services
{
    public interface IAuthorizationService
    {
        Task<IdentityResult> AddUserToRole(IdentityUser user, string role);
        Task<string> GetToken(IdentityUser user);
    }
}