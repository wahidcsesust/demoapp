using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HealthCare.Web.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using HealthCare.Data.Models;
using System;
using Microsoft.AspNetCore.Http;
using HealthCare.Web.Models.AccountViewModels;

namespace HealthCare.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Manage()
        {
            List<UserListViewModel> models = new List<UserListViewModel>();
            var users = _userManager.Users.Where(u => u.IsDeleted == false);
            models = users.Select(u => new UserListViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }).ToList();
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(string id)
        {
            UserViewModel model = new UserViewModel();
            model.Roles = _roleManager.Roles.Where(r => !r.IsDeleted).Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                User user = await _userManager.FindByIdAsync(id);
                var existingRoles = _roleManager.Roles.Count();
                if (user != null)
                {
                    model.Id = user.Id;
                    model.UserName = user.UserName;
                    model.Email = user.Email;
                    model.Password = user.PasswordHash;
                    model.ConfirmPassword = user.PasswordHash;
                    model.RoleId = existingRoles > 0 ? _roleManager.Roles.Single(r => r.Name == _userManager.GetRolesAsync(user).Result.Single()).Id : "";
                }
            }
            return View("EditorTemplates/User", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    string existingRole = _userManager.GetRolesAsync(user).Result.Single();
                    string existingRoleId = _roleManager.Roles.Single(r => r.Name == existingRole).Id;
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        if (existingRoleId != model.RoleId)
                        {
                            IdentityResult roleResult = await _userManager.RemoveFromRoleAsync(user, existingRole);
                            if (roleResult.Succeeded)
                            {
                                Role applicationRole = await _roleManager.FindByIdAsync(model.RoleId);
                                if (applicationRole != null)
                                {
                                    IdentityResult newRoleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                                    if (newRoleResult.Succeeded)
                                    {
                                        return RedirectToAction("Manage");
                                    }
                                }
                            }
                        }
                    }
                    else { AddErrors(result); }
                }
                else
                {
                    var aUser = new User { UserName = model.UserName, Email = model.Email };
                    IdentityResult result = await _userManager.CreateAsync(aUser, model.Password);
                    if (result.Succeeded)
                    {
                        Role applicationRole = await _roleManager.FindByIdAsync(model.RoleId);
                        if (applicationRole != null)
                        {
                            IdentityResult roleResult = await _userManager.AddToRoleAsync(aUser, applicationRole.Name);
                            if (roleResult.Succeeded)
                            {
                                return RedirectToAction("Manage");
                            }
                        }
                    }
                    else { AddErrors(result); }
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Manage");
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IActionResult> ChangePassword(string Id = null)
        {
            if (Id == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            User user = await _userManager.FindByIdAsync(Id);
            var model = new ChangeUserPasswordViewModel {
                Id = user.Id,
                UserName = user.UserName                
            };
            return View("EditorTemplates/ChangePassword", model);
        }

        [HttpPost]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangeUserPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("Manage");
            }
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            AddErrors(result);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            //string name = string.Empty;
            //if (!String.IsNullOrEmpty(id))
            //{
            //    User applicationUser = await _userManager.FindByIdAsync(id);
            //    if (applicationUser != null)
            //    {
            //        name = applicationUser.UserName;
            //    }
            //}
            ObjectDeleteViewModel model = new ObjectDeleteViewModel();
            if (!String.IsNullOrEmpty(id))
            {
                var applicationUser = await _userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    model.Name = applicationUser.UserName;
                }
            }
            return PartialView("DisplayTemplates/_DeleteUser", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectDeleteViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Name))
            {
                User applicationUser = await _userManager.FindByNameAsync(model.Name);
                if (applicationUser != null)
                {
                    //IdentityResult result = await _userManager.DeleteAsync(applicationUser);
                    applicationUser.IsDeleted = true; ;
                    IdentityResult result = await _userManager.UpdateAsync(applicationUser);
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