using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.Data.Interfaces;
using HealthCare.Data.Models;
using HealthCare.Web.Models.Accounting;
using HealthCare.Web.Models;

namespace HealthCare.Web.Controllers
{
    public class AccountingController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IAccountHeadRepository _accountHeadRepository;
        private readonly IAccountHeadTypeRepository _accountHeadTypeRepository;
        private readonly IAccountHeadHistoryRepository _accountHeadHistoryRepository;
        private readonly IMemberRepository _memberRepository;
        public AccountingController(IMemberRepository memberRepository,
            IAccountHeadRepository accountHeadRepository,
            IAccountHeadTypeRepository accountHeadTypeRepository,
            IAccountHeadHistoryRepository accountHeadHistoryRepository,
            UserManager<User> userManager)
        {
            _userManager = userManager;
            _accountHeadRepository = accountHeadRepository;
            _memberRepository = memberRepository;
            _accountHeadTypeRepository = accountHeadTypeRepository;
            _accountHeadHistoryRepository = accountHeadHistoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int id = 0)
        {
            var viewModel = new List<AccountHeadViewModel>();
            if (id == DateTime.Now.Year)
            {
                var accountHead = await _accountHeadRepository.GetAllActive();
                viewModel = accountHead.Select(m => new AccountHeadViewModel()
                {
                    Id = m.Id,
                    AccountHeadTypeName = _accountHeadTypeRepository.GetNameById(m.AccountHeadTypeId),
                    AccountHeadName = _accountHeadTypeRepository.Get(m.AccountHeadTypeId).Result.AcountType,
                    AccountNo = m.AccountNo,
                    Name = m.Name,
                    OpeningBalance = m.OpeningBalance,
                    CurrentBalance = m.CurrentBalance,
                    ActualCurrentBalance = m.ActualCurrentBalance
                }).ToList();
            }
            else
            {
                var accountHead = await _accountHeadHistoryRepository.GetAllActiveByYear(id);
                viewModel = accountHead.Select(m => new AccountHeadViewModel()
                {
                    Id = m.Id,
                    AccountHeadTypeName = _accountHeadTypeRepository.GetNameById(m.AccountHeadTypeId),
                    AccountHeadName = _accountHeadTypeRepository.Get(m.AccountHeadTypeId).Result.AcountType,
                    AccountNo = m.AccountNo,
                    Name = m.Name,
                    OpeningBalance = m.OpeningBalance,
                    CurrentBalance = m.CurrentBalance
                }).ToList();
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            var model = new AccountHeadViewModel();

            var accountHeadTypes = await _accountHeadTypeRepository.GetAll();
            model.AcconuntHeadTypes = accountHeadTypes.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.AcountType + "-" + x.Name
            }).ToList();

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var accountHead = await _accountHeadRepository.Get(id);
                if (accountHead != null)
                {
                    model.Id = accountHead.Id;
                    model.AccountHeadTypeId = accountHead.AccountHeadTypeId;
                    model.Name = accountHead.Name;
                    model.AccountNo = accountHead.AccountNo;
                    model.OpeningBalance = accountHead.OpeningBalance;
                    model.CurrentBalance = accountHead.CurrentBalance;
                    model.ActualCurrentBalance = accountHead.ActualCurrentBalance;
                }
            }

            return PartialView("EditorTemplates/_AccountHead", model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(AccountHeadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    var accountHead = await _accountHeadRepository.Get(model.Id);

                    if (accountHead == null)
                    {
                        accountHead = new AccountHead();
                        accountHead.AccountNo = _accountHeadTypeRepository.Get(model.AccountHeadTypeId).Result.AccountNo + "-" + _accountHeadRepository.GetNextAccountNo();
                    }
                    Random rnd = new Random();
                    accountHead.AccountHeadTypeId = model.AccountHeadTypeId;
                    accountHead.Name = model.Name;
                    accountHead.OpeningBalance = model.OpeningBalance;
                    accountHead.CurrentBalance = model.CurrentBalance;
                    accountHead.ActualCurrentBalance = model.ActualCurrentBalance;
                    accountHead.CreatedBy = user.Id;
                    accountHead.ModifiedBy = user.Id;

                    await _accountHeadRepository.Save(accountHead);
                    //return RedirectToAction("Manage");
                    return RedirectToAction("Manage", "Accounting", new { @id = DateTime.Today.Year });
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
                var accountHead = await _accountHeadRepository.Get(id);
                if (accountHead != null)
                {
                    model.Id = accountHead.Id;
                    model.Name = string.Empty;
                }
            }

            return PartialView("DisplayTemplates/_DeleteAccountHead", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectDeleteViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Id.ToString()) && model.Id != 0)
            {
                var accountHead = await _accountHeadRepository.Get(model.Id);
                if (accountHead != null)
                {
                    accountHead.IsDeleted = true; ;
                    await _accountHeadRepository.Update(accountHead);
                    //return RedirectToAction("Manage");
                    return RedirectToAction("Manage", "Accounting", new { @id = DateTime.Today.Year });
                }
            }
            return View();
        }
    }
}