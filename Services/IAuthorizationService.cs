using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace dev_ryan_iam.Services
{
    public interface IAuthorizationService
    {
        Task<IdentityResult> AddUserToRole(IdentityUser user, string role);
        Task<string> GetToken(IdentityUser user);
    }
}