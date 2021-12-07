using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HealthCare.Data.Models;
using Microsoft.AspNetCore.Identity;
using HealthCare.Web.Models;
using HealthCare.Data.Services;
using Microsoft.Extensions.Caching.Memory;
using HealthCare.Data.Common;
using System.Threading.Tasks;

namespace HealthCare.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<HelpCollection> _helpCollectionRepository;
        private readonly IGenericRepository<Collection> _collectionRepository;
        private readonly IGenericRepository<MiscellaneousCollection> _miscellaneousRepository;
        private readonly IGenericRepository<Expense> _expenseRepository;
        private IMemoryCache _cache;
        private readonly IGenericRepository<Loan> _loanRepository;
        private readonly IGenericRepository<LoanCollection> _loanCollectionRepository;
        private readonly IGenericRepository<CashBackCollection> _cashBackRepository;
        private readonly IGenericRepository<AccountHead> _accountHeadRepository;

        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IGenericRepository<Appointment> _appointmentRepository;
        private readonly IGenericRepository<PatientTest> _patientTestRepository;
        private readonly IGenericRepository<PatientAdmission> _patientAdmissionRepository;

        public HomeController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IGenericRepository<Member> memberRepository,
            IGenericRepository<HelpCollection> helpCollectionRepository,
            IGenericRepository<Collection> collectionRepository,
            IGenericRepository<MiscellaneousCollection> miscellaneousRepository,
            IGenericRepository<Expense> expenseRepository,
            IGenericRepository<Loan> loanRepository,
            IGenericRepository<LoanCollection> loanCollectionRepository,
            IGenericRepository<CashBackCollection> cashBackRepository,
            IMemoryCache memoryCache, 
            IGenericRepository<AccountHead> accountHeadRepository,
            IGenericRepository<Patient> patientRepository,
            IGenericRepository<Doctor> doctorRepository,
            IGenericRepository<Appointment> appointmentRepository,
            IGenericRepository<PatientTest> patientTestRepository,
            IGenericRepository<PatientAdmission> patientAdmissionRepository
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;

            _memberRepository = memberRepository;
            _helpCollectionRepository = helpCollectionRepository;
            _collectionRepository = collectionRepository;
            _miscellaneousRepository = miscellaneousRepository;
            _expenseRepository = expenseRepository;
            _loanRepository = loanRepository;
            _loanCollectionRepository = loanCollectionRepository;
            _cashBackRepository = cashBackRepository;
            _cache = memoryCache;
            _accountHeadRepository = accountHeadRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _patientTestRepository = patientTestRepository;
            _patientAdmissionRepository = patientAdmissionRepository;
        }
        public async Task<IActionResult> Index()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            //dashboardViewModel.NumberOfUser = _userManager.Users.LongCount();

            //var patients = await _patientRepository.GetAllActive();
            //dashboardViewModel.NumberOfPatient = patients.LongCount();

            //var doctors = await _doctorRepository.GetAllActive();
            //dashboardViewModel.NumberOfDoctor = doctors.LongCount();

            //var appointments = await _appointmentRepository.GetAllActive();
            //dashboardViewModel.NumberOfAppointment = appointments.LongCount();

            //var patientTests = await _patientTestRepository.GetAllActive();
            //dashboardViewModel.NumberOfDiagonosis = patientTests.LongCount();

            var objects = await _memberRepository.GetAllActive();
            //var mainBody = objects;
            dashboardViewModel.NumberOfAdvisor = objects.Where(m => m.Category == MemberCategoryEnum.AdvisoryCouncil.ToString()).LongCount();
            dashboardViewModel.NumberOfExecutive = objects.Where(m => m.Category == MemberCategoryEnum.ExecutiveCouncil.ToString()).LongCount();
            dashboardViewModel.NumberOfMember = objects.Where(m => m.Category == MemberCategoryEnum.GeneralMember.ToString()).LongCount();
            dashboardViewModel.NumberOfTotalMember = objects.LongCount();

            var helpCollections = await _helpCollectionRepository.GetAllActive();
            dashboardViewModel.NumberOfHelp = helpCollections.LongCount();

            //dashboardViewModel.NumberOfMainbody = mainBody.LongCount();


            //var admissions = await _patientAdmissionRepository.GetAllActive();
            //dashboardViewModel.NumberOfAdmission = admissions.LongCount();

            //var accountHeads = await _accountHeadRepository.GetAllActive();

            //dashboardViewModel.SanchayIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0010").CurrentBalance;
            //dashboardViewModel.OwnersEquityIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0011").ActualCurrentBalance??0;


            //var collections = await _collectionRepository.GetAllActive();
            //var todayCollections = collections.Where(c => c.DateOfCollection == Utility.GetCurrentDateString());
            //dashboardViewModel.TodaySanchayCollection = todayCollections.Where(c => !c.IsMainBody).Select(c => c.Amount).Sum();

            //var loanCollections = await _loanCollectionRepository.GetAllActive();
            //var todayLoanCollections = loanCollections.Where(c => c.DateOfCollection == Utility.GetCurrentDateString());
            //dashboardViewModel.TodayLoanCollection = todayLoanCollections.Select(c => c.CollectedAmount).Sum();

            ////Miscellaneous
            //dashboardViewModel.MemberBookIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-1-0012").CurrentBalance;
            //dashboardViewModel.BimaIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0013").CurrentBalance;
            //dashboardViewModel.OtherIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0014").CurrentBalance;
            //dashboardViewModel.MiscellaneousCollection = dashboardViewModel.MemberBookIncome + dashboardViewModel.BimaIncome + dashboardViewModel.OtherIncome;

            ////Expense
            //dashboardViewModel.UtilityExpense = accountHeads.FirstOrDefault(p => p.AccountNo == "4-1-0015").CurrentBalance;
            //dashboardViewModel.SanchayPrincipalCashback = accountHeads.FirstOrDefault(p => p.AccountNo == "4-1-0016").CurrentBalance;
            //dashboardViewModel.SanchayInterestCashback = accountHeads.FirstOrDefault(p => p.AccountNo == "4-1-0017").CurrentBalance;
            //dashboardViewModel.LoanDisburse = accountHeads.FirstOrDefault(p => p.AccountNo == "4-1-0018").CurrentBalance;

            //dashboardViewModel.ExpenseCollection = dashboardViewModel.UtilityExpense + dashboardViewModel.SanchayPrincipalCashback
            //    + dashboardViewModel.SanchayInterestCashback + dashboardViewModel.LoanDisburse;


            ////Today Collection

            //var miscellaneous = await _miscellaneousRepository.GetAllActive();
            //var expenses = await _expenseRepository.GetAllActive();
            //var cashBack = await _cashBackRepository.GetAllActive();



            //var todayMainBodyCollections = collections.Where(c => c.DateOfCollection == Utility.GetCurrentDateString());
            //dashboardViewModel.TodayMainBodyCollection = todayMainBodyCollections.Where(c => c.IsMainBody).Select(c => c.Amount).Sum();

            //var todayMiscellaneousCollections = miscellaneous.Where(c => c.DateOfCollection == Utility.GetCurrentDateString());
            //dashboardViewModel.TodayMiscellaneousCollection = todayMiscellaneousCollections.Select(c => c.Amount).Sum();

            //var todayExpenseCollections = expenses.Where(c => c.DateOfCollection == Utility.GetCurrentDateString());
            //dashboardViewModel.TodayExpenseCollection = todayExpenseCollections.Select(c => c.Amount).Sum();

            //var todayCashBackCollections = cashBack.Where(c => c.DateOfCollection == Utility.GetCurrentDateString());
            //dashboardViewModel.TodayCashBackCollection = todayCashBackCollections.Select(c => c.Amount).Sum();


            //dashboardViewModel.TodayCollection = dashboardViewModel.TodaySanchayCollection + dashboardViewModel.TodayMainBodyCollection
            //    + dashboardViewModel.TodayLoanCollection + dashboardViewModel.TodayMiscellaneousCollection - dashboardViewModel.TodayExpenseCollection
            //    - dashboardViewModel.TodayCashBackCollection;


            //// Loan

            //var loans = await _loanRepository.GetAllActive();
            //dashboardViewModel.NumberOfLoan = loans.LongCount();

            //dashboardViewModel.LoanAmount = accountHeads.FirstOrDefault(p => p.AccountNo == "4-1-0018").ActualCurrentBalance??0;

            //dashboardViewModel.LoanPrincipalReceivableIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0008").CurrentBalance;
            //dashboardViewModel.LoanInterestReceivableIncome = accountHeads.FirstOrDefault(p => p.AccountNo == "3-2-0009").CurrentBalance;

            //dashboardViewModel.LoanCollection = dashboardViewModel.LoanPrincipalReceivableIncome + dashboardViewModel.LoanInterestReceivableIncome;

            //dashboardViewModel.LoanPrincipleReceivable = accountHeads.FirstOrDefault(p => p.AccountNo == "1-3-0002").ActualCurrentBalance ?? 0;


            //// Account Balance

            //dashboardViewModel.PrettyCash = accountHeads.FirstOrDefault(p => p.AccountNo == "1-1-0001").ActualCurrentBalance ?? 0;

            //// Cash Back
            //dashboardViewModel.SanchayPrincipalCashback = accountHeads.FirstOrDefault(p => p.AccountNo == "4-1-0016").CurrentBalance;
            //dashboardViewModel.SanchayInterestCashback = accountHeads.FirstOrDefault(p => p.AccountNo == "4-1-0017").CurrentBalance;

            return View(dashboardViewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
