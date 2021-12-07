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

namespace HealthCare.Web.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IGenericRepository<Expense> _expenseRepository;
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IAccountHeadRepository accountHeadRepository;
        public ExpenseController(
            IGenericRepository<Member> memberRepository, 
            IGenericRepository<Expense> expenseRepository,
            UserManager<User> userManager,
            ICompositeViewEngine viewEngine,
            IAccountHeadRepository accountHeadRepository
            )
        {
            _userManager = userManager;
            _expenseRepository = expenseRepository;
            _memberRepository = memberRepository;
            _viewEngine = viewEngine;
            this.accountHeadRepository = accountHeadRepository;
        }
        public async Task<IActionResult> Manage()
        {
            var expense = await _expenseRepository.GetAllActive();

            var expenseViewModelList = expense.Select(m => new ExpenseViewModel()
            {
                Id = m.Id,
                InvoiceNo = m.InvoiceNo,
                Amount = m.Amount,
                DateOfCollection = Utility.GetDateFormatByValue(m.DateOfCollection),
                Particulars = m.Particulars
            }).ToList();
            return View(expenseViewModelList.OrderBy(r => r.InvoiceNo));
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            var model = new ExpenseViewModel()
            {
                DateOfCollection = Utility.GetCurrentDateString(),
            };

            return PartialView("EditorTemplates/_Expense", model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ExpenseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    var expense = new Expense()
                    {
                        InvoiceNo = model.InvoiceNo,
                        Amount = model.Amount,
                        CollectionDate = Utility.GetDateByValue(model.DateOfCollection),
                        DateOfCollection = model.DateOfCollection,
                        CreatedBy = user.Id,
                        ModifiedBy = user.Id,
                        Particulars = model.Particulars
                    };
                    await _expenseRepository.Save(expense);

                    await accountHeadRepository.UpdateAccountHeadCurrentBalance(EnumTransactionType.ExUtility.ToString(), model.Amount);

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
        public async Task<JsonResult> CheckInvoice(string invoiceNo = null)
        {
            Boolean isInvoiceNoExist = false;

            var objects = await _expenseRepository.GetAllActive();
            var expense = objects.Where(c => c.InvoiceNo == invoiceNo).FirstOrDefault();
            if (expense != null)
            {
                isInvoiceNoExist = true;
            }
            return Json(new { isInvoiceNoExist = isInvoiceNoExist });
        }

        [HttpGet]
        public async Task<PartialViewResult> Delete(long id = 0)
        {
            ObjectDeleteViewModel model = new ObjectDeleteViewModel();
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var expense = await _expenseRepository.Get(id);
                if (expense != null)
                {
                    model.Id = expense.Id;
                    model.Name = expense.InvoiceNo;
                }
            }

            return PartialView("DisplayTemplates/_DeleteExpense", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectDeleteViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Id.ToString()) && model.Id != 0)
            {
                var expense = await _expenseRepository.Get(model.Id);
                if (expense != null)
                {
                    expense.IsDeleted = true; ;
                    await _expenseRepository.Update(expense);
                    await accountHeadRepository.DeleteAccountHeadCurrentBalance(EnumTransactionType.ExUtility.ToString(), expense.Amount);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }


        [HttpGet]
        public async Task<JsonResult> PrintInvoice(long expenseId = 0)
        {
            if (expenseId > 0)
            {
                var expense = await _expenseRepository.Get(expenseId);

                ViewData["InvoiceNumber"] = expense != null ? expense.InvoiceNo : string.Empty;
                ViewData["Amount"] = expense != null ? expense.Amount.ToString() : string.Empty;
                ViewData["Date"] = Utility.GetDateFormatByValue(expense.DateOfCollection);
                ViewData["Particulars"] = expense != null ? expense.Particulars : string.Empty;
            }

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_PrintInvoice", null);
            return Json(new { html = renderedView });
        }
        private async Task<string> RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult =
                    _viewEngine.FindView(ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}