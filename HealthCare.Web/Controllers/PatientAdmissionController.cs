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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HealthCare.Web.Controllers
{
    public class PatientAdmissionController : Controller
    {
        private readonly IGenericRepository<PatientAdmission> _patientAdmissionRepository;
        private readonly IGenericRepository<Room> _roomRepository;
        private readonly IGenericRepository<Bed> _bedRepository;
        private readonly UserManager<User> _userManager;
        public PatientAdmissionController(IGenericRepository<PatientAdmission> patientAdmissionRepository, IGenericRepository<Room> roomRepository, IGenericRepository<Bed> bedRepository, UserManager<User> userManager)
        {
            _patientAdmissionRepository = patientAdmissionRepository;
            _roomRepository = roomRepository;
            _bedRepository = bedRepository;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Manage(int id = 0)
        {
            PatientAdmission model = new PatientAdmission();
            if(id != 0)
            {
                model = _patientAdmissionRepository.GetAllActive().Result.Where(a => a.Id == id).FirstOrDefault();
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Get(long id)
        {
            //var query = $"SELECT * FROM Departments WHERE Id=@Id";
            //var model = new Department();
            //using (var con = new SqlConnection(Constants.ConnectionString))
            //{
            //    model = con.Query<Department>(query, new { Id = id }).FirstOrDefault();
            //}
            PatientAdmission model = new PatientAdmission();
            if (id != 0)
            {
                model = _patientAdmissionRepository.GetAllActive().Result.Where(a => a.Id == id).FirstOrDefault();
            }

            return Json(model);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var query = $@"SELECT pa.Id, pt.Name PatientName, rm.RoomNo, rm.Location, pa.AdmissionDate FROM PatientAdmissions pa
                            left join Patients pt on pt.Id=pa.PatientId
                            left join Rooms rm on rm.Id=pa.RoomId
                            WHERE pa.IsDeleted=0 ORDER BY pa.Id";
            var modelList = new List<PatientAdmission>();
            using (var con = new SqlConnection(Constants.ConnectionString))
            {
                modelList = con.Query<PatientAdmission>(query).ToList();
            }

            return Json(new { modelList = modelList });
        }
        [HttpGet]
        public IActionResult GetSli()
        {
            var querySli = $@"select DISTINCT Id [Value], Name [Text] from Patients where IsDeleted=0
                            select DISTINCT Id [Value], Name [Text] from Doctors where IsDeleted=0 
                            select DISTINCT Id [Value], BedNo [Text] from Beds";

            var patientSli = new List<SelectListItem>();
            var doctorSli = new List<SelectListItem>();
            var bedSli = new List<SelectListItem>();
            using (var con = new SqlConnection(Constants.ConnectionString))
            {
                var data = con.QueryMultiple(querySli);
                patientSli = data.Read<SelectListItem>().ToList();
                doctorSli = data.Read<SelectListItem>().ToList();
                bedSli = data.Read<SelectListItem>().ToList();
            }
            //patientSli.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            //doctorSli.Insert(0, new SelectListItem { Value = "", Text = "Select" });

            return Json(new { patientSli = patientSli, doctorSli = doctorSli, bedSli = bedSli });
        }
        [HttpPost]
        public async Task<IActionResult> Save(PatientAdmission model)
        {
            var response = new ResponseModel();
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var _model = await _patientAdmissionRepository.Get(model.Id);

                model.AdmissionDate = model.AdmissionDate.ToString().StringToDate();

                model.CreatedBy = user.Id;
                model.ModifiedBy = user.Id;

                if (_model == null)
                {
                    model.CreatedDate = DateTime.Now;
                    model.ModifiedDate = DateTime.Now;
                }
                else
                {
                    model.ModifiedDate = DateTime.Now;
                    model.Id = _model.Id;
                }

                await _patientAdmissionRepository.Save(model);
                response.Success();
                response.Data = _model;
            }
            catch (Exception ex)
            {
                response.Error(details: ex.Message);
            }

            return Json(response);
        }

        //[HttpPost]
        //public async Task<IActionResult> Delete(long id)
        //{
        //    var response = new ResponseModel();
        //    try
        //    {
        //        if (!String.IsNullOrEmpty(id.ToString()))
        //        {
        //            var department = await _departmentRepository.Get(id);
        //            if (department != null)
        //            {
        //                department.IsDeleted = true; ;
        //                await _departmentRepository.Save(department);
        //                response.Success();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Error(details: ex.Message);
        //    }
        //    return Json(response);
        //}
    }
}