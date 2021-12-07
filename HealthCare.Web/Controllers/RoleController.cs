using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using HealthCare.Web.Models;

namespace HealthCare.Web.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public RoleController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public IActionResult Manage()
        {
            List<RoleListViewModel> models = new List<RoleListViewModel>();
            var roles = _roleManager.Roles.Where(u => u.IsDeleted == false);
            models = roles.Select(r => new RoleListViewModel
            {
                RoleName = r.Name,
                Id = r.Id
            }).ToList();

            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(string id)
        {
            RoleViewModel model = new RoleViewModel();
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                Role applicationRole = await _roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    model.Id = applicationRole.Id;
                    model.RoleName = applicationRole.Name;
                }
            }
            return View("EditorTemplates/Role", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isExist = !String.IsNullOrEmpty(model.Id) && !model.Id.Equals("0");
                Role applicationRole = isExist ? await _roleManager.FindByIdAsync(model.Id) : new Role();
                applicationRole.Name = model.RoleName;
                IdentityResult result = isExist ? await _roleManager.UpdateAsync(applicationRole)
                                                    : await _roleManager.CreateAsync(applicationRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("Manage");
                }
                else { AddErrors(result); }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id))
            {
                Role applicationRole = await _roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    name = applicationRole.Name;
                }
            }
            return PartialView("DisplayTemplates/_DeleteRole", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, FormCollection form)
        {
            if (!String.IsNullOrEmpty(id))
            {
                Role applicationRole = await _roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    //IdentityResult roleRuslt = _roleManager.DeleteAsync(applicationRole).Result;
                    applicationRole.IsDeleted = true; ;
                    IdentityResult result = await _roleManager.UpdateAsync(applicationRole);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage");
                    }
                }
            }
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}