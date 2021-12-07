using HealthCare.Data.Models;
using HealthCare.Data.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare.Data.Interfaces
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        string GetNameById(long Id);
        string GetRegNoById(long Id);
        decimal GetTotalAmountById(long Id);
        Task<decimal> GetMainBodyAmountById(long Id);

        decimal GetMainBodyCollectionById(long Id);
        Task<IEnumerable<Member>> GetMemberListByCategoryId(string strCategory, string strAreaLocation, string strBloodGroup);
    }
}
