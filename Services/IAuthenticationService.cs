using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace dev_ryan_iam.Services
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> DeleteAccount(string email, string password);
        Task<IdentityResult> Register(string userName, string email, string password);
        Task<IdentityUser> SignIn(string email, string password);
    }
}