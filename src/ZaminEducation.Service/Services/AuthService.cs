using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Domain.Enums;
using ZaminEducation.Service.Exceptions;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> repository;
        private readonly IConfiguration configuration;

        public AuthService(IRepository<User> repository, IConfiguration configuration)
        {
            this.repository = repository;
            this.configuration = configuration;
        }

        public async Task<string> GenerateToken(string username, string password)
        {
            User user = await repository.GetAsync(u =>
                u.Username == username && u.Password == password && u.State != ItemState.Deleted);

            if (user is null)
                throw new ZaminEducationException(400, "Login or Password is incorrect");

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] tokenKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);

            SecurityTokenDescriptor tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                Issuer = configuration["JWT:Issuer"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}
