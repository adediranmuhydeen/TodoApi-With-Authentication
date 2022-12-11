using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TodoWebApp.Share;

namespace TodoWebApp.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);

    }


    public class UserService : IUserService
    {
        private IConfiguration _configuration;
        private UserManager<IdentityUser> _userManager;

        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }



        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
                throw new NullReferenceException("Register Model is Null");

            if (model.Password != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "Confirm Password doesn't match",
                    IsSuccess = false,
                };

            var IdenttityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email

            };

            var result = await _userManager.CreateAsync(IdenttityUser, model.Password);

            if (result.Succeeded)
            {
                // Send a confirmation Email
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "User Successfully creayted",

                };

            }

            return new UserManagerResponse
            {
                Message = "User was not created",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };


        }
        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "Email Does not exist",
                    IsSuccess = false,
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Wrong Password",
                    IsSuccess = false,
                };
            }

            var claim = new[]
            {
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claim,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            string tokenAssString = new JwtSecurityTokenHandler().WriteToken(token);
            return new UserManagerResponse
            {
                Message = tokenAssString,
                IsSuccess = false,
                ExpireDate = token.ValidTo
            };
        }
    }
}
