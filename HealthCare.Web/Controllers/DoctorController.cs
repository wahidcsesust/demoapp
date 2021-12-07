using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Models;
using HealthCare.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HealthCare.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using HealthCare.Web.Models.Doctor;

namespace HealthCare.Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly UserManager<User> _userManager;
        public DoctorController(IGenericRepository<Doctor> doctorRepository, IGenericRepository<Department> departmentRepository, UserManager<User> userManager)
        {
            _doctorRepository = doctorRepository;
            _departmentRepository = departmentRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var objects = await _doctorRepository.GetAll();

            var doctors = objects.Where(d => d.IsDeleted == false);

            var doctorViewModelList = new List<DoctorViewModel>();
            foreach (var doctor in doctors)
            {
                var doctorViewModel = new DoctorViewModel();
                doctorViewModel.Id = doctor.Id;
                doctorViewModel.Name = doctor.Name;
                doctorViewModel.BmdcRegNumber = doctor.BmdcRegNumber;
                doctorViewModel.Designation = doctor.Designation;
                doctorViewModel.Gender = doctor.Gender;
                doctorViewModel.MobileNumber = doctor.MobileNumber;
                doctorViewModel.EmailAddress = doctor.EmailAddress;
                doctorViewModel.Specilization = doctor.Specilization;
                doctorViewModel.PhoneNumber = doctor.PhoneNumber;
                doctorViewModel.DegreeOther = doctor.DegreeOther;
                doctorViewModel.DepartmentId = doctor.DepartmentId;
                doctorViewModel.VisitPrice = doctor.VisitPrice;
                doctorViewModel.DepartmentName = doctor.Department.Name;
                doctorViewModelList.Add(doctorViewModel);
            }

            return View(doctorViewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            DoctorViewModel model = new DoctorViewModel();
            var departments = await _departmentRepository.GetAllActive();
            model.Departments = departments.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var doctor = await _doctorRepository.Get(id);
                if (doctor != null)
                {
                    model.Name = doctor.Name;
                    model.BmdcRegNumber = doctor.BmdcRegNumber;
                    model.Designation = doctor.Designation;
                    model.Gender = doctor.Gender;
                    model.MobileNumber = doctor.MobileNumber;
                    model.EmailAddress = doctor.EmailAddress;
                    model.Specilization = doctor.Specilization;
                    model.PhoneNumber = doctor.PhoneNumber;
                    model.DegreeOther = doctor.DegreeOther;
                    model.DepartmentId = doctor.DepartmentId;
                    model.VisitPrice = doctor.VisitPrice;
                }
            }
            return View("EditorTemplates/Doctor", model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(DoctorViewModel model)
        {
            var errors = ModelState
               .Where(x => x.Value.Errors.Count > 0)
               .Select(x => new { x.Key, x.Value.Errors })
               .ToArray();
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    var doctor = await _doctorRepository.Get(model.Id);
                    if(doctor == null)
                    {
                        doctor = new Doctor();
                    }
                    doctor.Name = model.Name;
                    doctor.BmdcRegNumber = model.BmdcRegNumber;
                    doctor.Designation = model.Designation;
                    doctor.Gender = model.Gender;
                    doctor.MobileNumber = model.MobileNumber;
                    doctor.EmailAddress = model.EmailAddress;
                    doctor.Specilization = model.Specilization;
                    doctor.PhoneNumber = model.PhoneNumber;
                    doctor.DegreeOther = model.DegreeOther;
                    doctor.DepartmentId = model.DepartmentId;
                    doctor.VisitPrice = model.VisitPrice;

                    doctor.CreatedBy = user.Id;
                    doctor.ModifiedBy = user.Id;
                    await _doctorRepository.Save(doctor);
                    return RedirectToAction("Manage");
                }
                catch (DbUpdateException ex )
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator." + ex.Message.ToString());
                }
            }
            // If we got this far, something failed, redisplay form
            var departments = await _departmentRepository.GetAllActive();
            model.Departments = departments.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return View("EditorTemplates/Doctor", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var doctor = await _doctorRepository.Get(id);
                if (doctor != null)
                {
                    name = doctor.Name;
                }
            }
            return PartialView("DisplayTemplates/_DeleteDoctor", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectDeleteViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Id.ToString()) && model.Id != 0)
            {
                var doctor = await _doctorRepository.Get(model.Id);
                if (doctor != null)
                {
                    doctor.IsDeleted = true; ;
                    await _doctorRepository.Update(doctor);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
    }
}