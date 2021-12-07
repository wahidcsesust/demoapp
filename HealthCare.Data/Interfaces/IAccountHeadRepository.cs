using HealthCare.Data.Models;
using HealthCare.Data.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare.Data.Interfaces
{
    public interface IAccountHeadRepository : IGenericRepository<AccountHead>
    {
        string GetNextAccountNo();
        Task<(int totalRecords, List<AccountHead>)> UpdateAccountHeadCurrentBalance(string transactionType, decimal amount);
        Task<(int totalRecords, List<AccountHead>)> DeleteAccountHeadCurrentBalance(string transactionType, decimal amount);
    }
}
