using CityProblems.DataAccess.Entities;

namespace CityProblems.Services
{
    public interface ICategoryService
    {
        Task<CategoryEntity?> Get(string id);
    }
}