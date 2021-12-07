using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using HealthCare.Data.Models;
using HealthCare.Data.Services;
using Microsoft.AspNetCore.Identity;
using HealthCare.Web.Models.Supplier;

namespace HealthCare.Web.Controllers
{
    public class SupplierController : Controller
    {
        private readonly IGenericRepository<Supplier> _supplierRepository;
        private readonly UserManager<User> _userManager;

        public SupplierController(IGenericRepository<Supplier> supplierRepository, UserManager<User> userManager)
        {
            _supplierRepository = supplierRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Manage()
        {
            var objects = await _supplierRepository.GetAll();

            var suppliers = objects.Where(s => !s.IsDeleted).ToList();

            var supplierViewModelList = new List<SupplierViewModel>();
            foreach (var supplier in suppliers)
            {

                var supplierViewModel = new SupplierViewModel();
                supplierViewModel.Id = supplier.Id;
                supplierViewModel.Code = supplier.Code;
                supplierViewModel.SupName = supplier.SupName;
                supplierViewModel.SupAddress = supplier.SupAddress;
                supplierViewModel.OfficePhone = supplier.OfficePhone;
                supplierViewModel.OfficeMail = supplier.OfficeMail;
                supplierViewModel.WebAddress = supplier.WebAddress;
                supplierViewModel.ContactPerson = supplier.ContactPerson;
                supplierViewModel.ContactPersonAddress = supplier.ContactPersonAddress;
                supplierViewModel.ContactPersonMobile = supplier.ContactPersonMobile;
                supplierViewModel.ContactPersonMail = supplier.ContactPersonMail;
                supplierViewModel.BankAccountName = supplier.BankAccountName;
                supplierViewModel.BankAccountNumber = supplier.BankAccountNumber;
                supplierViewModel.Remarks = supplier.Remarks;
                supplierViewModel.IsPaymentBeforeSale = supplier.IsPaymentBeforeSale;
                supplierViewModel.PaymentDay = supplier.PaymentDay;
                supplierViewModel.IsPaymentAfterSale = supplier.IsPaymentAfterSale;
                supplierViewModel.Status = supplier.Status;
                supplierViewModelList.Add(supplierViewModel);
            }
            return View(supplierViewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            SupplierViewModel supplierViewModel = new SupplierViewModel();

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var supplier = await _supplierRepository.Get(id);
                if (supplier != null)
                {
                    supplierViewModel.Id = supplier.Id;
                    supplierViewModel.Code = supplier.Code;
                    supplierViewModel.SupName = supplier.SupName;
                    supplierViewModel.SupAddress = supplier.SupAddress;
                    supplierViewModel.OfficePhone = supplier.OfficePhone;
                    supplierViewModel.OfficeMail = supplier.OfficeMail;
                    supplierViewModel.WebAddress = supplier.WebAddress;
                    supplierViewModel.ContactPerson = supplier.ContactPerson;
                    supplierViewModel.ContactPersonAddress = supplier.ContactPersonAddress;
                    supplierViewModel.ContactPersonMobile = supplier.ContactPersonMobile;
                    supplierViewModel.ContactPersonMail = supplier.ContactPersonMail;
                    supplierViewModel.BankAccountName = supplier.BankAccountName;
                    supplierViewModel.BankAccountNumber = supplier.BankAccountNumber;
                    supplierViewModel.Remarks = supplier.Remarks;
                    supplierViewModel.IsPaymentBeforeSale = supplier.IsPaymentBeforeSale;
                    supplierViewModel.PaymentDay = supplier.PaymentDay;
                    supplierViewModel.IsPaymentAfterSale = supplier.IsPaymentAfterSale;
                    supplierViewModel.Status = supplier.Status;

                }
            }

            return View("EditorTemplates/Supplier", supplierViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save(SupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var supplier = await _supplierRepository.Get(model.Id);
                    var user = await _userManager.GetUserAsync(User);
                    if (supplier != null)
                    {
                        supplier.Code = model.Code;
                        supplier.SupName = model.SupName;
                        supplier.SupAddress = model.SupAddress;
                        supplier.OfficePhone = model.OfficePhone;
                        supplier.OfficeMail = model.OfficeMail;
                        supplier.WebAddress = model.WebAddress;
                        supplier.ContactPerson = model.ContactPerson;
                        supplier.ContactPersonAddress = model.ContactPersonAddress;
                        supplier.ContactPersonMobile = model.ContactPersonMobile;
                        supplier.ContactPersonMail = model.ContactPersonMail;
                        supplier.BankAccountName = model.BankAccountName;
                        supplier.BankAccountNumber = model.BankAccountNumber;
                        supplier.Remarks = model.Remarks;
                        supplier.IsPaymentBeforeSale = model.IsPaymentBeforeSale;
                        supplier.PaymentDay = model.PaymentDay;
                        supplier.IsPaymentAfterSale = model.IsPaymentAfterSale;
                        supplier.Status = model.Status;
                        supplier.IsActive = true;
                        supplier.ModifiedDate = DateTime.Now;
                        supplier.ModifiedBy = user.Id;
                        await _supplierRepository.Update(supplier);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        supplier = new Supplier();
                        supplier.Code = model.Code;
                        supplier.SupName = model.SupName;
                        supplier.SupAddress = model.SupAddress;
                        supplier.OfficePhone = model.OfficePhone;
                        supplier.OfficeMail = model.OfficeMail;
                        supplier.WebAddress = model.WebAddress;
                        supplier.ContactPerson = model.ContactPerson;
                        supplier.ContactPersonAddress = model.ContactPersonAddress;
                        supplier.ContactPersonMobile = model.ContactPersonMobile;
                        supplier.ContactPersonMail = model.ContactPersonMail;
                        supplier.BankAccountName = model.BankAccountName;
                        supplier.BankAccountNumber = model.BankAccountNumber;
                        supplier.Remarks = model.Remarks;
                        supplier.IsPaymentBeforeSale = model.IsPaymentBeforeSale;
                        supplier.PaymentDay = model.PaymentDay;
                        supplier.IsPaymentAfterSale = model.IsPaymentAfterSale;
                        supplier.Status = model.Status;
                        supplier.IsActive = true;
                        supplier.CreatedBy = user.Id;
                        supplier.CreatedDate = DateTime.Now;
                        supplier.ModifiedDate = DateTime.Now;
                        supplier.ModifiedBy = user.Id;
                        await _supplierRepository.Insert(supplier);
                        return RedirectToAction("Manage");
                    }
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator." + ex.Message.ToString());
                }
            }

            return View("EditorTemplates/Supplier", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var supplier = await _supplierRepository.Get(id);
                if (supplier != null)
                {
                    name = supplier.SupName;
                }
            }

            return PartialView("DisplayTemplates/_DeleteSupplier", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id, FormCollection form)
        {
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var supplier = await _supplierRepository.Get(id);
                if (supplier != null)
                {
                    //var result = _supplierRepository.Delete(supplier);
                    supplier.IsDeleted = true; ;
                    await _supplierRepository.Update(supplier);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
    }
}