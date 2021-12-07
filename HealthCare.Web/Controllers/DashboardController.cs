using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using HealthCare.Data.Models;
using HealthCare.Data.Services;
using HealthCare.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Controllers
{
    public class DashboardController : Controller
    {
        private IMemoryCache _cache;
        private readonly IGenericRepository<AccountHead> _accountHeadRepository;

        public DashboardController(
            IMemoryCache memoryCache, IGenericRepository<AccountHead> accountHeadRepository)
        {
            _accountHeadRepository = accountHeadRepository;
            _cache = memoryCache;
        }
        public async Task<IActionResult> Index()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel();

            var accountHeads = await _accountHeadRepository.GetAllActive();
            //Asset
            dashboardViewModel.PrettyCash = accountHeads.FirstOrDefault(p => p.AccountNo == "1-1-0001").ActualCurrentBalance ?? 0;
            //dashboardViewModel.LoanPrincipleReceivable = accountHeads.FirstOrDefault(p => p.AccountNo == "1-3-0002").ActualCurrentBalance ?? 0;
            //dashboardViewModel.LoanInterestReceivable = accountHeads.FirstOrDefault(p => p.AccountNo == "1-3-0003").ActualCurrentBalance ?? 0;
            //dashboardViewModel.SecurityAdvanceReceivable = accountHeads.FirstOrDefault(p => p.AccountNo == "1-3-0019").ActualCurrentBalance ?? 0;
            dashboardViewModel.MedicalPaymentReceivable = accountHeads.FirstOrDefault(p => p.AccountNo == "1-3-0006").ActualCurrentBalance ?? 0;
            ////Liability
            //dashboardViewModel.SanchayPrincipalPayable = accountHeads.FirstOrDefault(p => p.AccountNo == "2-1-0004").ActualCurrentBalance ?? 0;
            //dashboardViewModel.OwnersEquity = accountHeads.FirstOrDefault(p => p.AccountNo == "2-3-0019").ActualCurrentBalance ?? 0;
            //dashboardViewModel.BimaPayable = accountHeads.FirstOrDefault(p => p.AccountNo == "2-1-0019").ActualCurrentBalance ?? 0;
            //dashboardViewModel.SanchayInterestPayable = accountHeads.FirstOrDefault(p => p.AccountNo == "2-1-0007").ActualCurrentBalance ?? 0;
            ////Income
            //dashboardViewModel.LoanPrincipalReceivableIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0008").CurrentBalance;
            //dashboardViewModel.LoanInterestReceivableIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0009").CurrentBalance;
            //dashboardViewModel.SanchayIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0010").CurrentBalance;
            //dashboardViewModel.OwnersEquityIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0011").CurrentBalance;
            //dashboardViewModel.MemberBookIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-1-0012").CurrentBalance;
            //dashboardViewModel.BimaIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0013").CurrentBalance;
            //dashboardViewModel.OtherIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0014").CurrentBalance;
            dashboardViewModel.DoctorVisitIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0002").CurrentBalance;
            dashboardViewModel.MedicalTestIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0003").CurrentBalance;
            dashboardViewModel.AdmissionIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0007").CurrentBalance;
            dashboardViewModel.PharmacyIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-1-0004").CurrentBalance;
            ////Expense
            dashboardViewModel.UtilityExpense = accountHeads.FirstOrDefault(p => p.AccountNo == "4-1-0005").CurrentBalance;
            dashboardViewModel.LessAmountExpense = accountHeads.FirstOrDefault(p => p.AccountNo == "4-1-0008").CurrentBalance;
            //dashboardViewModel.SanchayPrincipalCashback = accountHeads.FirstOrDefault(p => p.AccountNo == "4-1-0016").CurrentBalance;
            //dashboardViewModel.SanchayInterestCashback = accountHeads.FirstOrDefault(p => p.AccountNo == "4-1-0017").CurrentBalance;
            //dashboardViewModel.LoanDisburse = accountHeads.FirstOrDefault(p => p.AccountNo == "4-1-0018").CurrentBalance;



            return View(dashboardViewModel);
        }
    }
}