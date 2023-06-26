using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<IdentityResult> AddUserAsync(User user, string password);

    }
}
