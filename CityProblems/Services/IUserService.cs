using CityProblems.DataAccess.Entities;

namespace CityProblems.Services
{
    public interface IUserService
    {
        Task<UserEntity?> Create(string username, string password, string email);
        Task<UserEntity?> Get(string id);
        Task<UserEntity?> Get(string username, string password);
        Task<bool> Authorize(UserEntity user, HttpContext context);
        Task<List<UserEntity>> GetList();
    }
}