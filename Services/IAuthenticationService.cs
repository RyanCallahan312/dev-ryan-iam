using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace auto_highlighter_iam.Services
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> Register(string userName, string email, string password);
        Task<IdentityUser> SignIn(string email, string password);
    }
}