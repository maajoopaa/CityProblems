using CityProblems.DataAccess;
using CityProblems.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityProblems.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CityProblemsDbContext _context;

        public CategoryService(CityProblemsDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryEntity?> Get(string id)
        {
            try
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == Guid.Parse(id));

                return category;
            }
            catch
            {
                return null;
            }
        }
    }
}
