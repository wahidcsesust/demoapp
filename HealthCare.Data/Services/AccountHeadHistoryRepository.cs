using Microsoft.EntityFrameworkCore;
using HealthCare.Data.Interfaces;
using HealthCare.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Data.Services
{
    public class AccountHeadHistoryRepository : GenericRepository<AccountHeadHistory>, IAccountHeadHistoryRepository
    {
        private readonly ApplicationDbContext context;
        public AccountHeadHistoryRepository(ApplicationDbContext _context) : base(_context)
        {
            this.context = _context;
        }
        public async Task<IEnumerable<AccountHeadHistory>> GetAllActiveByYear(int year = 0)
        {
            return await context.AccountHeadHistory.Where(t => !t.IsDeleted && t.ClosingYear == year).ToListAsync();
        }
    }
}
