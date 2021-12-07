using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HealthCare.Data.Models;
using Microsoft.AspNetCore.Http;
using HealthCare.Web.Models.Customer;

namespace HealthCare.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly UserManager<User> _userManager;

        public CustomerController(IGenericRepository<Customer> CustomerRepository, UserManager<User> userManager)
        {
            _customerRepository = CustomerRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Manage()
        {
            var objects = await _customerRepository.GetAll();

            var customers = objects.Where(c => !c.IsDeleted);

            var customerViewModelList = new List<CustomerViewModel>();
            foreach (var customer in customers)
            {

                var customerViewModel = new CustomerViewModel();
                customerViewModel.Id = customer.Id;
                customerViewModel.Code = customer.Code;
                customerViewModel.Name = customer.Name;
                customerViewModel.AccountManagerCode = customer.AccountManagerCode;
                customerViewModel.AccountManagerName = customer.AccountManagerName;
                customerViewModel.Address = customer.Address;
                customerViewModel.PostCode = customer.PostCode;
                customerViewModel.MobileNumber = customer.MobileNumber;
                customerViewModel.PhoneNumber = customer.PhoneNumber;
                customerViewModel.MailAddress = customer.MailAddress;
                customerViewModel.DeliveryDate = customer.DeliveryDate;
                customerViewModel.Remarks = customer.Remarks;
                customerViewModel.Status = customer.Status;
                customerViewModelList.Add(customerViewModel);
            }
            return View(customerViewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            CustomerViewModel customerViewModel = new CustomerViewModel();

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var customer = await _customerRepository.Get(id);
                if (customer != null)
                {
                    customerViewModel.Id = customer.Id;
                    customerViewModel.Code = customer.Code;
                    customerViewModel.Name = customer.Name;
                    customerViewModel.AccountManagerCode = customer.AccountManagerCode;
                    customerViewModel.AccountManagerName = customer.AccountManagerName;
                    customerViewModel.Address = customer.Address;
                    customerViewModel.PostCode = customer.PostCode;
                    customerViewModel.MobileNumber = customer.MobileNumber;
                    customerViewModel.PhoneNumber = customer.PhoneNumber;
                    customerViewModel.MailAddress = customer.MailAddress;
                    customerViewModel.DeliveryDate = customer.DeliveryDate;
                    customerViewModel.Remarks = customer.Remarks;
                    customerViewModel.Status = customer.Status;
                }
            }

            return View("EditorTemplates/Customer", customerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customer = await _customerRepository.Get(model.Id);
                    var user = await _userManager.GetUserAsync(User);
                    if (customer != null)
                    {
                        customer.Code = model.Code;
                        customer.Code = model.Code;
                        customer.Name = model.Name;
                        customer.AccountManagerCode = model.AccountManagerCode;
                        customer.AccountManagerName = model.AccountManagerName;
                        customer.Address = model.Address;
                        customer.PostCode = model.PostCode;
                        customer.MobileNumber = model.MobileNumber;
                        customer.PhoneNumber = model.PhoneNumber;
                        customer.MailAddress = model.MailAddress;
                        customer.DeliveryDate = model.DeliveryDate;
                        customer.Remarks = model.Remarks;
                        customer.Status = model.Status;
                        customer.IsActive = true;
                        customer.ModifiedDate = DateTime.Now;
                        customer.ModifiedBy = user.Id;
                        await _customerRepository.Update(customer);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        customer = new Customer();
                        customer.Code = model.Code;
                        customer.Code = model.Code;
                        customer.Name = model.Name;
                        customer.AccountManagerCode = model.AccountManagerCode;
                        customer.AccountManagerName = model.AccountManagerName;
                        customer.Address = model.Address;
                        customer.PostCode = model.PostCode;
                        customer.MobileNumber = model.MobileNumber;
                        customer.PhoneNumber = model.PhoneNumber;
                        customer.MailAddress = model.MailAddress;
                        customer.DeliveryDate = model.DeliveryDate;
                        customer.Remarks = model.Remarks;
                        customer.Status = model.Status;
                        customer.IsActive = true;
                        customer.CreatedDate = DateTime.Now;
                        customer.CreatedBy = user.Id;
                        customer.ModifiedDate = DateTime.Now;
                        customer.ModifiedBy = user.Id;
                        await _customerRepository.Insert(customer);
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

            return View("EditorTemplates/Customer", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var Customer = await _customerRepository.Get(id);
                if (Customer != null)
                {
                    name = Customer.Name;
                }
            }

            return PartialView("DisplayTemplates/_DeleteCustomer", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id, FormCollection form)
        {
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var customer = await _customerRepository.Get(id);
                if (customer != null)
                {
                    //var result = _customerRepository.Delete(Customer);
                    customer.IsDeleted = true; ;
                    await _customerRepository.Update(customer);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
    }
}