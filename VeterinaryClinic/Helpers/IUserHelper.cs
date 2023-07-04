using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

    }
}
