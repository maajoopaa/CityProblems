using CityProblems.DataAccess.Entities;

namespace CityProblems.Services
{
    public interface IMessageService
    {
        bool Send(UserEntity receiver, UserEntity sender, IssueEntity issue, bool isForWorker=false);
    }
}