using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserHelper(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        /// <summary>
        /// Método para criar um novo user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>user, password</returns>
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
        /// <summary>
        /// Método para adicionar o role do user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns>user, roleName</returns>
        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
        /// <summary>
        /// Método para alterar o password.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns>user, oldPassword, newPassword</returns>
        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }
        /// <summary>
        /// Método para chegar se o role existe.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>rolename</returns>
        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });

            }
        }
        /// <summary>
        /// Método para confirmar email.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns>user, token</returns>
        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }
        /// <summary>
        /// Método para gerar um email de confirmação.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>emailconfirmation</returns>
        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
        /// <summary>
        /// Método para modificar a password com token.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>emailpassword</returns>
        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        /// <summary>
        /// Método que retorna um user através do seu email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>email</returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        /// <summary>
        /// Método que retorna o user através do seu id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>userId</returns>
        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        /// <summary>
        /// Método que verifica se o user já possui um role.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns>user, roleName</returns>
        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }
        /// <summary>
        /// Método que retorna a subscrição de um user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Username, Password, RememberMe</returns>
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
               model.Username,
               model.Password,
               model.RememberMe,
               false);
        }
        /// <summary>
        /// Método que faz o logout do user.
        /// </summary>
        /// <returns>Desloga</returns>
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        /// <summary>
        /// Método que modifica a password.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <param name="password"></param>
        /// <returns>user, token, password</returns>
        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }
        /// <summary>
        /// Método que realiza o update do user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>user</returns>
        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }
        /// <summary>
        /// Método que faz a validação da password.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>user, password</returns>
        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false);
        }
    }
}
