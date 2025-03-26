using CityProblems.DataAccess.Entities;
using CityProblems.Models;

namespace CityProblems.Services
{
    public interface IIssueService
    {
        Task<IssueEntity?> Create(string category, string description, string latitude, string longitude, byte[] image, UserEntity createdBy);
        Task<IssueEntity?> Get(string id);
        Task<bool> Remove(string id);
        Task<bool> Update(IssueEntity issue);
        Task<List<IssueEntity>> GetList();
    }
}