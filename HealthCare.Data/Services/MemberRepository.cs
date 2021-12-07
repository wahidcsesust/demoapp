using Microsoft.EntityFrameworkCore;
using HealthCare.Data.Interfaces;
using HealthCare.Data.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HealthCare.Data.Services
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        private readonly ApplicationDbContext context;
        public MemberRepository(ApplicationDbContext _context) : base(_context)
        {
            this.context = _context;
        }

        public List<Member> GetMembersByCategory(string catId)
        {
            var members = context.Members.Where(m => m.Category == catId).ToList();
            return members;
        }

        public async Task<IEnumerable<Member>> GetMemberListByCategoryId(string strCategory, string strAreaLocation, string strBloodGroup)
        {
            var memberList = await context.Members.Where(m => m.IsDeleted == false 
            && (m.Category == strCategory || strCategory == "All") 
            && (m.AreaLocation == strAreaLocation || strAreaLocation == "All")
            && (m.BloodGroup == strBloodGroup || strBloodGroup == "All")
            ).ToListAsync();

            return memberList;

            //var members = await context.Members.Where(m => m.IsDeleted == false).ToListAsync();

            //var result = members.FindAll(a => a.Category == strCategory || strCategory == "All");

            //var resultAreaLocationList = result.FindAll(a => a.AreaLocation == strAreaLocation || strAreaLocation == "All");

            //var resultBloodList = resultAreaLocationList.FindAll(a => a.BloodGroup == strBloodGroup || strBloodGroup == "All");

            //return resultBloodList;
        }
        ////////////////////public string GetMemberListByCategoryId(long Id)
        ////////////////////{
        ////////////////////    var name = context.Members.FirstOrDefaultAsync(m => m.Category == Id).Result.Name;
        ////////////////////    return name;
        ////////////////////}

        public string GetNameById(long Id)
        {
            var name = context.Members.FirstOrDefaultAsync(m => m.Id == Id).Result.Name;
            return name;
        }
        public string GetRegNoById(long Id)
        {
            var regNo = context.Members.FirstOrDefaultAsync(m => m.Id == Id).Result.RegNo;
            return regNo.ToString().PadLeft(6, '0');
        }
        public decimal GetTotalAmountById(long Id)
        {
            var amount = 0; //context.Members.FirstOrDefaultAsync(m => m.Id == Id).Result.TotalAmount;
            return amount;
        }
        public async Task<decimal> GetMainBodyAmountById(long Id)
        {
            //var query = await (from zz in context.Collections
            //                  where zz.Foo > 1
            //                  group zz by 1 into grp
            //                  select new
            //                  {
            //                      rowCount = grp.CountAsync(),
            //                      rowSum = grp.SumAsync(x => x.SomeColumn)
            //                  }).ToListAsync();

            var amount = await (from aa in context.Collections
                                where aa.MemberId == Id
                                group aa by 1 into grp
                                select new
                                {
                                    rowSum = grp.Sum(x => x.Amount)
                                }).SingleAsync();

            var result = amount.rowSum;
            return result;
        }
        public decimal GetMainBodyCollectionById(long Id)
        {
            var amount = from aa in context.Collections
                                where aa.MemberId == Id && !aa.IsDeleted && aa.IsMainBody
                                group aa by 1 into grp
                                select new
                                {
                                    rowSum = grp.Sum(x => x.Amount)
                                };

            var result = amount.Select(a => a.rowSum).FirstOrDefault();
            return result;
        }
    }
}
