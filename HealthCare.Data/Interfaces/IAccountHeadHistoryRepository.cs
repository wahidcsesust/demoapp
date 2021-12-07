using HealthCare.Data.Models;
using HealthCare.Data.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare.Data.Interfaces
{
    public interface IAccountHeadHistoryRepository : IGenericRepository<AccountHeadHistory>
    {
        Task<IEnumerable<AccountHeadHistory>> GetAllActiveByYear(int year = 0);
    }
}
