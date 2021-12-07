using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Services;
using Microsoft.AspNetCore.Identity;
using HealthCare.Data.Models;
using HealthCare.Web.Models;
using System.Data.SqlClient;
using HealthCare.Web.Services;
using Dapper;
using System.Threading.Tasks;

namespace HealthCare.Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly UserManager<User> _userManager;
        public DepartmentController(IGenericRepository<Department> departmentRepository, IGenericRepository<Doctor> doctorRepository, UserManager<User> userManager)
        {
            _departmentRepository = departmentRepository;
            _doctorRepository = doctorRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var query = $"SELECT * FROM Departments WHERE IsDeleted=0 ORDER BY NAME";
            var modelList = new List<Department>();
            using (var con = new SqlConnection(Constants.ConnectionString))
            {
                modelList = con.Query<Department>(query).ToList();
            }

            return Json(modelList);
        }
        [HttpGet]
        public IActionResult Get(long id)
        {
            var query = $"SELECT * FROM Departments WHERE Id=@Id";
            var model = new Department();
            using (var con = new SqlConnection(Constants.ConnectionString))
            {
                model = con.Query<Department>(query, new { Id = id }).FirstOrDefault();
            }

            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> Save(Department model)
        {
            var response = new ResponseModel();
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var department = await _departmentRepository.Get(model.Id);

                if (department == null)
                {
                    department = new Department();
                    department.CreatedBy = user.Id;
                }
                department.ModifiedBy = user.Id;
                department.Name = model.Name;

                await _departmentRepository.Save(department);
                response.Success();
                response.Data = department;
            }
            catch (Exception ex)
            {
                response.Error(details: ex.Message);
            }

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var response = new ResponseModel();
            try
            {
                if (!String.IsNullOrEmpty(id.ToString()))
                {
                    var department = await _departmentRepository.Get(id);
                    if (department != null)
                    {
                        department.IsDeleted = true; ;
                        await _departmentRepository.Save(department);
                        response.Success();
                    }
                }
            }
            catch (Exception ex)
            {
                response.Error(details: ex.Message);
            }
            return Json(response);
        }

        //[HttpGet]
        //public async Task<IActionResult> Manage()
        //{
        //    List<DepartmentViewModel> models = new List<DepartmentViewModel>();
        //    var objects = await _departmentRepository.GetAll();

        //    var departments = objects.Where(r => r.IsDeleted == false);
        //    models = departments.Select(d => new DepartmentViewModel
        //    {
        //        Id = d.Id,
        //        Name = d.Name,
        //        NumberOfDoctors = d.Doctors.LongCount()
        //    }).ToList();

        //    return View(models);
        //}

        //[HttpGet]
        //public async Task<IActionResult> CreateEdit(long id = 0)
        //{
        //    DepartmentViewModel model = new DepartmentViewModel();
        //    if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
        //    {
        //        var department = await _departmentRepository.Get(id);
        //        if(department != null)
        //        {
        //            model.Name = department.Name;
        //        }
        //    }
        //    return PartialView("EditorTemplates/_Department", model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Save(DepartmentViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.GetUserAsync(User);
        //        try
        //        {
        //            var department = await _departmentRepository.Get(model.Id);
        //            if (department != null)
        //            {
        //                department.Name = model.Name;
        //                department.IsActive = true;
        //                department.ModifiedDate = DateTime.Now;
        //                department.ModifiedBy = user.Id;
        //                await _departmentRepository.Update(department);
        //                return RedirectToAction("Manage");
        //            }
        //            else
        //            {
        //                Department aDepartment = new Department();
        //                aDepartment.Name = model.Name;
        //                aDepartment.IsActive = true;
        //                aDepartment.CreatedDate = DateTime.Now;
        //                aDepartment.CreatedBy = user.Id;
        //                aDepartment.ModifiedDate = DateTime.Now;
        //                aDepartment.ModifiedBy = user.Id;

        //                await _departmentRepository.Insert(aDepartment);
        //                return RedirectToAction("Manage");
        //            }
        //        }
        //        catch (DbUpdateException ex)
        //        {
        //            //Log the error (uncomment ex variable name and write a log.
        //            ModelState.AddModelError("", "Unable to save changes. " +
        //                "Try again, and if the problem persists " +
        //                "see your system administrator." + ex.Message.ToString());
        //        }
        //    }
        //    return View("EditorTemplates/Doctor", model);
        //}

        //[HttpGet]
        //public async Task<IActionResult> Delete(long id)
        //{
        //    string name = string.Empty;
        //    if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
        //    {
        //        var department = await _departmentRepository.Get(id);
        //        if (department != null)
        //        {
        //            name = department.Name;
        //        }
        //    }
        //    return PartialView("DisplayTemplates/_DeleteDepartment", name);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(ObjectDeleteViewModel model)
        //{
        //    if (!String.IsNullOrEmpty(model.Id.ToString()) && model.Id != 0)
        //    {
        //        var department = await _departmentRepository.Get(model.Id);
        //        if (department != null)
        //        {
        //            department.IsDeleted = true; ;
        //            await _departmentRepository.Update(department);
        //            return RedirectToAction("Manage");
        //        }
        //    }
        //    return View();
        //}
    }
}