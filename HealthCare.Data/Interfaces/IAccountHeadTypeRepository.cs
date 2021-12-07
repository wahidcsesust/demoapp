using HealthCare.Data.Models;
using HealthCare.Data.Services;

namespace HealthCare.Data.Interfaces
{
    public interface IAccountHeadTypeRepository : IGenericRepository<AccountHeadType>
    {
        string GetNameById(long Id);
    }
}
