using CityProblems.DataAccess;
using CityProblems.DataAccess.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace CityProblems.Services
{
    public class UserService : IUserService
    {
        private readonly CityProblemsDbContext _context;
        private readonly IMessageService _messageService;

        public UserService(CityProblemsDbContext context, IMessageService messageService)
        {
            _context = context;
            _messageService = messageService;
        }

        public async Task<UserEntity?> Create(string username, string password, string email)
        {
            try
            {
                var userEntity = new UserEntity
                {
                    Username = username,
                    Password = password,
                    Email = email
                };

                await _context.Users
                    .AddAsync(userEntity);

                await _context.SaveChangesAsync();

                return userEntity;
            }
            catch
            {
                return null;
            }
        }

        public async Task<UserEntity?> Get(string id)
        {
            try
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == Guid.Parse(id));

                return user;
            }
            catch
            {
                return null;
            }
        }

        public async Task<UserEntity?> Get(string username, string password)
        {
            try
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

                return user;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Authorize(UserEntity user, HttpContext context)
        {
            try
            {
				var claims = new List<Claim> {
					new(ClaimTypes.NameIdentifier, user.Id.ToString())
				};

				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

				var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await context.SignInAsync(claimsPrincipal);

                return true;
			}
            catch
            {
                return false;
            }
        }

        public async Task<List<UserEntity>> GetList()
        {
            try
            {
                var users = await _context.Users
                    .AsNoTracking()
                    .ToListAsync();

                return users ?? new List<UserEntity>();
            }
            catch
            {
                return new List<UserEntity>();
            }
        }
    }
}
