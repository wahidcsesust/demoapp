using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Services;
using Microsoft.AspNetCore.Identity;
using HealthCare.Data.Models;
using HealthCare.Web.Models.StaffViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace HealthCare.Web.Controllers
{
    public class StaffController : Controller
    {
        
        private readonly IGenericRepository<Staff> _staffRepository;
        private readonly IGenericRepository<StaffPicture> _staffPictureRepository;
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<EducationList> _educationListRepository;
        private readonly IGenericRepository<BankList> _banklistRepository;
        private readonly IGenericRepository<StaffSalaryStructureDetails> _staffSalaryStructureDetailsRepository;
        private readonly IGenericRepository<StaffWiseOtherSalaryDetails> _staffWiseOtherSalaryDetailsRepository;
        private readonly UserManager<User> _userManager;


        public StaffController(IGenericRepository<Staff> staffRepository, IGenericRepository<StaffPicture> staffPictureRepository, IGenericRepository<StaffSalaryStructureDetails> staffSalaryStructureDetailsRepository,IGenericRepository<StaffWiseOtherSalaryDetails> staffWiseOtherSalaryDetailsRepository, IGenericRepository<Department> departmentRepository, IGenericRepository<EducationList> educationListRepository, IGenericRepository<BankList> bankListRepository, UserManager<User> userManager)
        {
            _staffRepository = staffRepository;
            _staffPictureRepository = staffPictureRepository;
            _departmentRepository = departmentRepository;
            _staffSalaryStructureDetailsRepository = staffSalaryStructureDetailsRepository;
            _staffWiseOtherSalaryDetailsRepository = staffWiseOtherSalaryDetailsRepository;
            _educationListRepository = educationListRepository;
            _banklistRepository = bankListRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var objects = await _staffRepository.GetAll();

            var staffs = objects.Where(d => d.IsDeleted == false);

            var StaffViewModelList = new List<StaffViewModel>();
            foreach (var staff in staffs)
            {
                var department = new Department();
                var education = new EducationList();
                var banklist = new BankList();
                var StaffViewModel = new StaffViewModel();
                StaffViewModel.Id = staff.Id;
                StaffViewModel.StaffId = staff.Id;
                StaffViewModel.StaffName = staff.StaffName;
                StaffViewModel.DateOfBirth = staff.DateOfBirth.ToString();
                StaffViewModel.JoinDate = staff.JoinDate.ToString();
                StaffViewModel.Address = staff.Address;
                StaffViewModel.Gender = staff.Gender;
                StaffViewModel.LastEducation = staff.LastEducation;
                StaffViewModel.MobileNumber = staff.MobileNumber;
                StaffViewModel.DepartmentId = staff.DepartmentId;
                StaffViewModel.Designation = staff.Designation;
                StaffViewModel.BankAccountNumber = staff.BankAccountNumber;
                StaffViewModel.BankAccountName = staff.BankAccountName;
                StaffViewModel.BankId = staff.BankId;
                StaffViewModel.NationalID = staff.NationalId;
                department = await _departmentRepository.Get(staff.DepartmentId);
                education = await _educationListRepository.Get(staff.LastEducation);
                banklist = await _banklistRepository.Get(staff.BankId);
                //banklist = await _banklistRepository.Get(staff.BankId);
                StaffViewModel.DepartmentName = department != null ? department.Name : string.Empty;
                StaffViewModel.LastEducationName = education != null ? education.Name : string.Empty;
                StaffViewModel.BankName = banklist != null ? banklist.BankName : string.Empty;
                StaffViewModelList.Add(StaffViewModel);
            }

            return View(StaffViewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            StaffViewModel model = new StaffViewModel();
            var departments = await _departmentRepository.GetAll();
            var educations = await _educationListRepository.GetAll();
            var banklists = await _banklistRepository.GetAll();
            model.Departments = departments.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            model.LastEducations = educations.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            model.Banks = banklists.Select(x => new SelectListItem
            {
                Text = x.BankName,
                Value = x.Id.ToString()
            }).ToList();

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var staff = await _staffRepository.Get(id);
                if (staff != null)
                {
                    model.StaffId = staff.StaffId;
                    model.StaffName = staff.StaffName;
                    model.JoinDate = staff.JoinDate.ToString();
                    model.DateOfBirth = staff.DateOfBirth.ToString();
                    model.Address = staff.Address;
                    model.EmailAddress = staff.EmailAddress;
                    model.Gender = staff.Gender;
                    model.LastEducation = staff.LastEducation;
                    model.MobileNumber = staff.MobileNumber;
                    model.PhoneNumber = staff.PhoneNumber;
                    model.DepartmentId = staff.DepartmentId;
                    model.Designation = staff.Designation;
                    model.BankAccountNumber = staff.BankAccountNumber;
                    model.BankAccountName = staff.BankAccountName;
                    model.BankId = staff.BankId;
                    model.NationalID = staff.NationalId;
                }
            }
            return View("EditorTemplates/Staff", model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(StaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    var staff = await _staffRepository.Get(model.Id);
                    if (staff != null)
                    {
                        staff.StaffId = model.StaffId;
                        staff.StaffName = model.StaffName;
                        staff.JoinDate = model.JoinDate.ToString();
                        staff.DateOfBirth = model.DateOfBirth.ToString();
                        staff.Address = model.Address;
                        staff.EmailAddress = model.EmailAddress;
                        staff.Gender = model.Gender;
                        staff.LastEducation = model.LastEducation;
                        staff.MobileNumber = model.MobileNumber;
                        staff.PhoneNumber = model.PhoneNumber;
                        staff.DepartmentId = model.DepartmentId;
                        staff.Designation = model.Designation;
                        staff.BankAccountNumber = model.BankAccountNumber;
                        staff.BankAccountName = model.BankAccountName;
                        staff.BankId = model.BankId;
                        staff.NationalId = model.NationalID;
                        staff.IsActive = true;
                        staff.ModifiedDate = DateTime.Now;
                        staff.ModifiedBy = user.Id;
                        await _staffRepository.Update(staff);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        var aStaff = new Staff();
                        aStaff.StaffId = model.StaffId;
                        aStaff.StaffName = model.StaffName;
                        aStaff.JoinDate =model.JoinDate.ToString();
                        aStaff.DateOfBirth = model.DateOfBirth.ToString();
                        aStaff.Address = model.Address;
                        aStaff.EmailAddress = model.EmailAddress;
                        aStaff.Gender = model.Gender;
                        aStaff.LastEducation = model.LastEducation;
                        aStaff.MobileNumber = model.MobileNumber;
                        aStaff.PhoneNumber = model.PhoneNumber;
                        aStaff.DepartmentId = model.DepartmentId;
                        aStaff.Designation = model.Designation;
                        aStaff.BankAccountNumber = model.BankAccountNumber;
                        aStaff.BankAccountName = model.BankAccountName;
                        aStaff.BankId = model.BankId;
                        aStaff.NationalId = model.NationalID;

                        aStaff.IsActive = true;
                        aStaff.CreatedDate = DateTime.Now;
                        aStaff.CreatedBy = user.Id;
                        aStaff.ModifiedDate = DateTime.Now;
                        aStaff.ModifiedBy = user.Id;

                        await _staffRepository.Insert(aStaff);
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
            var departments = await _departmentRepository.GetAll();
            model.Departments = departments.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var educations = await _educationListRepository.GetAll();
            model.LastEducations = educations.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var banklists = await _banklistRepository.GetAll();
            model.Banks = banklists.Select(x => new SelectListItem
            {
                Text = x.BankName,
                Value = x.Id.ToString()
            }).ToList();
            return View("EditorTemplates/Staff", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var staff = await _staffRepository.Get(id);
                if (staff != null)
                {
                    name = staff.StaffName;
                }
            }
            return PartialView("DisplayTemplates/_DeleteStaff", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id, FormCollection form)
        {
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var staff = await _staffRepository.Get(id);
                if (staff != null)
                {
                    //var result = _staffRepository.Delete(staff);
                    staff.IsDeleted = true; ;
                    await _staffRepository.Update(staff);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetEmployeeImage(long id)
        {
            var objects = await _staffRepository.GetAll();           
            var staff = objects.Where(d => d.IsDeleted == false && d.Id == id).FirstOrDefault();

            var staffPictureModel = new List<StaffPicture>();            
                var staffImage = new StaffPictureViewModel();
                staffImage.Id = staff.Id;
                staffImage.StaffId = staff.StaffId;
            // staffImage.StaffImage = staff.sta;
            return View();
        }
    }
}