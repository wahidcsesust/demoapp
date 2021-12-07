using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Models;
using HealthCare.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using HealthCare.Web.Models.Product;
using Microsoft.AspNetCore.Identity;

namespace NextCloudPos.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductGroup> _productGroupRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly UserManager<User> _userManager;

        public ProductController(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository, IGenericRepository<ProductGroup> productGroupRepository, IGenericRepository<ProductBrand> productBrandRepository, UserManager<User> userManager)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productGroupRepository = productGroupRepository;
            _productBrandRepository = productBrandRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Manage()
        {
            var objects = await _productRepository.GetAll();

            var products = objects.Where(a => !a.IsDeleted);

            var productViewModelList = new List<ProductViewModel>();
            foreach (var product in products)
            {
                var category = new Category();
                var productGroup = new ProductGroup();
                var productBrand = new ProductBrand();
                var productViewModel = new ProductViewModel();
                productViewModel.Id = product.Id;
                productViewModel.Code = product.Code;
                productViewModel.Name = product.Name;
                productViewModel.MeasurementType = product.MeasurementType;
                productViewModel.MeasurementUnit = product.MeasurementUnit;
                productViewModel.CostPrice = product.CostPrice;
                productViewModel.SalePrice = product.SalePrice;

                category = await _categoryRepository.Get(product.CategoryId);
                productViewModel.CategoryName = category != null ? category.Name : string.Empty;

                productGroup = await _productGroupRepository.Get(product.ProductGroupId);
                productViewModel.ProductGroupName = productGroup != null ? productGroup.Name : string.Empty;

                productBrand = await _productBrandRepository.Get(product.ProductBrandId);
                productViewModel.ProductBrandName = productBrand != null ? productBrand.Name : string.Empty;

                productViewModelList.Add(productViewModel);
            }
            return View(productViewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            ProductViewModel model = new ProductViewModel();
            var categories = await _categoryRepository.GetAll();
            model.Categories = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var productGroups = await _productGroupRepository.GetAll();
            model.ProductGroups = productGroups.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var productBrands = await _productBrandRepository.GetAll();
            model.ProductBrands = productBrands.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var product = await _productRepository.Get(id);
                if (product != null)
                {
                    model.Code = product.Code;
                    model.Name = product.Name;
                    model.CategoryId = product.CategoryId;
                    model.ProductGroupId = product.ProductGroupId;
                    model.ProductBrandId = product.ProductBrandId;
                    model.MeasurementType = product.MeasurementType;
                    model.MeasurementUnit = product.MeasurementUnit;
                    model.CostPrice = product.CostPrice;
                    model.SalePrice = product.SalePrice;
                }
            }

            return View("EditorTemplates/Product", model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _productRepository.Get(model.Id);
                    var user = await _userManager.GetUserAsync(User);
                    if (product != null)
                    {
                        product.Code = model.Code;
                        product.Name = model.Name;
                        product.CategoryId = model.CategoryId;
                        product.ProductGroupId = model.ProductGroupId;
                        product.ProductBrandId = model.ProductBrandId;
                        product.MeasurementType = model.MeasurementType;
                        product.MeasurementUnit = model.MeasurementUnit;
                        product.CostPrice = model.CostPrice;
                        product.SalePrice = model.SalePrice;
                        product.IsActive = true;
                        product.ModifiedDate = DateTime.Now;
                        product.ModifiedBy = user.Id;
                        await _productRepository.Update(product);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        var aProduct = new Product();
                        aProduct.Code = model.Code;
                        aProduct.Name = model.Name;
                        aProduct.CategoryId = model.CategoryId;
                        aProduct.ProductGroupId = model.ProductGroupId;
                        aProduct.ProductBrandId = model.ProductBrandId;
                        aProduct.MeasurementType = model.MeasurementType;
                        aProduct.MeasurementUnit = model.MeasurementUnit;
                        aProduct.CostPrice = model.CostPrice;
                        aProduct.SalePrice = model.SalePrice;
                        aProduct.IsActive = true;
                        aProduct.CreatedDate = DateTime.Now;
                        aProduct.ModifiedDate = DateTime.Now;
                        aProduct.CreatedBy = user.Id;
                        aProduct.ModifiedBy = user.Id;
                        await _productRepository.Insert(aProduct);
                        return RedirectToAction("Manage");
                    }
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }

            var categories = await _categoryRepository.GetAll();
            model.Categories = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var productGroups = await _productGroupRepository.GetAll();
            model.ProductGroups = productGroups.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var productBrands = await _productBrandRepository.GetAll();
            model.ProductBrands = productBrands.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return View("EditorTemplates/Product", model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var product = await _productRepository.Get(id);
                if (product != null)
                {
                    name = product.Name;
                }
            }

            return PartialView("DisplayTemplates/_DeleteProduct", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id, FormCollection form)
        {
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var product = await _productRepository.Get(id);
                if (product != null)
                {
                    //var result = _productRepository.Delete(product);
                    product.IsDeleted = true;
                    await _productRepository.Update(product);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
    }
}