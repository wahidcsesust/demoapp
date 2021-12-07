using HealthCare.Data.Interfaces;
using HealthCare.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Data.Services
{
    public class AccountHeadTypeRepository : GenericRepository<AccountHeadType>, IAccountHeadTypeRepository
    {
        private readonly ApplicationDbContext context;
        public AccountHeadTypeRepository(ApplicationDbContext _context) : base(_context)
        {
            this.context = _context;
        }
        public string GetNameById(long Id)
        {
            var name = context.AccountHeadTypes.FirstOrDefaultAsync(m => m.Id == Id).Result.Name;
            return name;
        }
    }
}
