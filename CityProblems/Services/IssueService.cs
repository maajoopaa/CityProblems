using CityProblems.DataAccess;
using CityProblems.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityProblems.Services
{
    public class IssueService : IIssueService
    {
        private readonly CityProblemsDbContext _context;

        public IssueService(CityProblemsDbContext context)
        {
            _context = context;
        }

        public async Task<IssueEntity?> Create(string categoryId, string description, string latitude, string longitude,
            byte[] image, UserEntity createdBy)
        {
            try
            {
                var issueEntity = new IssueEntity
                {
                    CategoryId = Guid.Parse(categoryId),
                    Description = description,
                    Latitude = latitude,
                    Longitude = longitude,
                    Image = image,
                    ExecutionState = ExecutionState.Sent,
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = createdBy.Id,
                };

                await _context.Issues
                    .AddAsync(issueEntity);

                await _context.SaveChangesAsync();

                return issueEntity;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IssueEntity?> Get(string id)
        {
            try
            {
                var issue = await _context.Issues
                    .FirstOrDefaultAsync(i => i.Id == Guid.Parse(id));

                return issue;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Update(IssueEntity issue)
        {
            try
            {
                if (issue is not null)
                {
                    await _context.Issues
                        .Where(i => i.Id == issue.Id)
                        .ExecuteUpdateAsync(s => s
                        .SetProperty(i => i.CategoryId, issue.CategoryId)
                        .SetProperty(i => i.Description, issue.Description)
                        .SetProperty(i => i.Latitude, issue.Latitude)
                        .SetProperty(i => i.Longitude, issue.Longitude)
                        .SetProperty(i => i.Image, issue.Image)
                        .SetProperty(i => i.ExecutionState, issue.ExecutionState));

                    await _context.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Remove(string id)
        {
            try
            {
                var issue = await Get(id);

                if (issue is not null)
                {
                    _context.Issues.Remove(issue);

                    await _context.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<IssueEntity>> GetList()
        {
            try
            {
                var issues = await _context.Issues
                    .AsNoTracking()
                    .ToListAsync();

                return issues ?? new List<IssueEntity>();
            }
            catch
            {
                return new List<IssueEntity>();
            }
        }
    }
}
