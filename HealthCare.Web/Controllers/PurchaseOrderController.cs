using HealthCare.Data.Models;
using HealthCare.Data.Services;
using HealthCare.Web.Models.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly IGenericRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IGenericRepository<Supplier> _supplierRepository;
        private readonly UserManager<User> _userManager;

        public PurchaseOrderController(
            IGenericRepository<PurchaseOrder> purchaseOrderRepository,
            IGenericRepository<Supplier> supplierRepository,
            UserManager<User> userManager)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _supplierRepository = supplierRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var objects = await _purchaseOrderRepository.GetAll();

            var purchaseOrders = objects.Where(a => !a.IsDeleted);

            var purchaseGroupViewModelList = new List<PurchaseOrderViewModel>();
            foreach (var purchaseOrder in purchaseOrders)
            {
                var supplier = new Supplier();
                var purchaseGroupViewModel = new PurchaseOrderViewModel();
                purchaseGroupViewModel.Id = purchaseOrder.Id;
                purchaseGroupViewModel.PONumber = purchaseOrder.PONumber;
                purchaseGroupViewModel.ReceivedByDate = purchaseOrder.ReceivedByDate;
                supplier = await _supplierRepository.Get(purchaseOrder.SupplierId);
                purchaseGroupViewModel.SupplierName = supplier != null ? supplier.SupName : string.Empty;
                purchaseGroupViewModelList.Add(purchaseGroupViewModel);
            }

            return View(purchaseGroupViewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            PurchaseOrderViewModel model = new PurchaseOrderViewModel();
            var suppliers = await _supplierRepository.GetAll();
            model.Suppliers = suppliers.Select(x => new SelectListItem
            {
                Text = x.SupName,
                Value = x.Id.ToString()
            }).ToList();
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var purchaseOrder = await _purchaseOrderRepository.Get(id);
                if (purchaseOrder != null)
                {
                    model.PONumber = purchaseOrder.PONumber;
                    model.SupplierId = purchaseOrder.SupplierId;
                    model.OrderedBy = purchaseOrder.OrderedBy;
                    model.ReceivedByDate = purchaseOrder.ReceivedByDate;
                    model.Status = purchaseOrder.Status;
                    model.Comments = purchaseOrder.Comments;
                    model.BookedBy = purchaseOrder.BookedBy;
                    model.BookedByDate = purchaseOrder.BookedByDate;
                }
            }
            return View("EditorTemplates/PurchaseOrder", model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(PurchaseOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var purchaseOrder = await _purchaseOrderRepository.Get(model.Id);
                    var user = await _userManager.GetUserAsync(User);
                    if (purchaseOrder != null)
                    {
                        purchaseOrder.PONumber = model.PONumber;
                        purchaseOrder.SupplierId = model.SupplierId;
                        purchaseOrder.OrderedBy = model.OrderedBy;
                        purchaseOrder.ReceivedByDate = model.ReceivedByDate;
                        purchaseOrder.Status = model.Status;
                        purchaseOrder.Comments = model.Comments;
                        purchaseOrder.BookedBy = model.BookedBy;
                        purchaseOrder.BookedByDate = model.BookedByDate;
                        purchaseOrder.IsActive = true;
                        purchaseOrder.ModifiedDate = DateTime.Now;
                        purchaseOrder.ModifiedBy = user.Id;
                        await _purchaseOrderRepository.Update(purchaseOrder);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        var aPurchaseOrder = new PurchaseOrder();
                        aPurchaseOrder.PONumber = model.PONumber;
                        aPurchaseOrder.SupplierId = model.SupplierId;
                        aPurchaseOrder.OrderedBy = model.OrderedBy;
                        aPurchaseOrder.ReceivedByDate = model.ReceivedByDate;
                        aPurchaseOrder.Status = model.Status;
                        aPurchaseOrder.Comments = model.Comments;
                        aPurchaseOrder.BookedBy = model.BookedBy;
                        aPurchaseOrder.BookedByDate = model.BookedByDate;
                        aPurchaseOrder.IsActive = true;
                        aPurchaseOrder.CreatedDate = DateTime.Now;
                        aPurchaseOrder.ModifiedDate = DateTime.Now;
                        aPurchaseOrder.CreatedBy = user.Id;
                        aPurchaseOrder.ModifiedBy = user.Id;
                        await _purchaseOrderRepository.Insert(aPurchaseOrder);
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

            // If we got this far, something failed, redisplay form
            var suppliers = await _supplierRepository.GetAll();
            model.Suppliers = suppliers.Select(x => new SelectListItem
            {
                Text = x.SupName,
                Value = x.Id.ToString()
            }).ToList();

            return View("EditorTemplates/PurchaseOrder", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var purchaseOrder = await _purchaseOrderRepository.Get(id);
                if (purchaseOrder != null)
                {
                    name = purchaseOrder.PONumber;
                }
            }

            return PartialView("DisplayTemplates/_DeletePurchaseOrder", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id, FormCollection form)
        {
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var purchaseOrder = await _purchaseOrderRepository.Get(id);
                if (purchaseOrder != null)
                {
                    //var result = _productGroupRepository.Delete(productGroup);
                    purchaseOrder.IsDeleted = true; ;
                    await _purchaseOrderRepository.Update(purchaseOrder);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
    }
}