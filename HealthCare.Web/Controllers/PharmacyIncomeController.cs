using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using HealthCare.Data.Common;
using HealthCare.Data.Interfaces;
using HealthCare.Data.Models;
using HealthCare.Data.Services;
using HealthCare.Web.Models;
using HealthCare.Web.Models.Enums;
using HealthCare.Web.Models.Expenses;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HealthCare.Web.Services;

namespace HealthCare.Web.Controllers
{
    public class PharmacyIncomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IGenericRepository<Expense> _expenseRepository;
        private readonly IGenericRepository<PharmacyIncome> _pharmacyIncomeRepository;
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IAccountHeadRepository accountHeadRepository;
        public PharmacyIncomeController(
            IGenericRepository<Member> memberRepository,
            IGenericRepository<Expense> expenseRepository,
            IGenericRepository<PharmacyIncome> pharmacyIncomeRepository,
            UserManager<User> userManager,
            ICompositeViewEngine viewEngine,
            IAccountHeadRepository accountHeadRepository
            )
        {
            _userManager = userManager;
            _expenseRepository = expenseRepository;
            _pharmacyIncomeRepository = pharmacyIncomeRepository;
            _memberRepository = memberRepository;
            _viewEngine = viewEngine;
            this.accountHeadRepository = accountHeadRepository;
        }
        public async Task<IActionResult> Manage()
        {
            var model = await _pharmacyIncomeRepository.GetAllActive();

            var modelList = model.Select(m => new PharmacyIncome()
            {
                Id = m.Id,
                InvoiceNo = m.InvoiceNo,
                Amount = m.Amount,
                CollectionDateString = m.CollectionDate != null ? m.CollectionDate.Value.ToString(Constants.DateFormat) : "",
                Particulars = m.Particulars
            }).ToList();
            return View(modelList.OrderBy(r => r.InvoiceNo));
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new PharmacyIncome()
            {
                CollectionDateString = DateTime.Now.ToString(Constants.DateFormat),
            };

            return PartialView("EditorTemplates/_PharmacyIncome", model);
        }
        [HttpPost]
        public async Task<IActionResult> Save(PharmacyIncome model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    var _model = new PharmacyIncome()
                    {
                        InvoiceNo = model.InvoiceNo,
                        Amount = model.Amount,
                        CollectionDate = model.CollectionDateString.StringToDate(),
                        CreatedBy = user.Id,
                        ModifiedBy = user.Id,
                        Particulars = model.Particulars
                    };
                    await _pharmacyIncomeRepository.Save(_model);

                    await accountHeadRepository.UpdateAccountHeadCurrentBalance(EnumTransactionType.PhIncUtility.ToString(), model.Amount);

                    return RedirectToAction("Manage");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator." + ex.Message.ToString());
                }
            }

            return View("Manage", model);
        }
        [HttpGet]
        public async Task<PartialViewResult> Delete(long id = 0)
        {
            ObjectDeleteViewModel model = new ObjectDeleteViewModel();
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var expense = await _pharmacyIncomeRepository.Get(id);
                if (expense != null)
                {
                    model.Id = expense.Id;
                    model.Name = expense.InvoiceNo;
                }
            }

            return PartialView("DisplayTemplates/_DeletePharmacyIncome", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectDeleteViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Id.ToString()) && model.Id != 0)
            {
                var _model = await _pharmacyIncomeRepository.Get(model.Id);
                if (_model != null)
                {
                    _model.IsDeleted = true; ;
                    await _pharmacyIncomeRepository.Update(_model);
                    await accountHeadRepository.DeleteAccountHeadCurrentBalance(EnumTransactionType.PhIncUtility.ToString(), _model.Amount);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
    }
}