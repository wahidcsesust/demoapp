using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Services;
using HealthCare.Data.Models;
using Microsoft.AspNetCore.Identity;
using HealthCare.Web.Models.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using HealthCare.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using HealthCare.Data.Common;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;

namespace HealthCare.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IGenericRepository<Patient> _patientsRepository;
        private readonly UserManager<User> _userManager;
        public PatientController(IGenericRepository<Patient> patientsRepository, UserManager<User> userManager)
        {
            _patientsRepository = patientsRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var patients = await _patientsRepository.GetAllActive();

            var patientViewModelList = patients.Select(m => new PatientsViewModel()
            {
                Id = m.Id,
                RegNo = m.RegNo,
                Name = m.Name,
                Age = m.Age,
                Weight = m.Weight,
                Gender = m.Gender,
                DateOfBirth = Utility.GetDateFormatByValue(m.DateOfBirth),
                BloodGroupEnum = Utility.GetBloodGroup(m.BloodGroup),
                MobileNumber = m.MobileNumber,
                Address = m.Address
            }).ToList();

            return View(patientViewModelList.OrderBy(r => r.RegNo));
        }

        [HttpGet]
        public async Task<PartialViewResult> CreateEdit(long id = 0, string type = null)
        {
            PatientsViewModel model = new PatientsViewModel() {
                DateOfBirth = Utility.GetCurrentDateString()
            };
            model.ModalType = type;

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var patient = await _patientsRepository.Get(id);
                if (patient != null)
                {
                    model.RegNo = patient.RegNo;
                    model.Name = patient.Name;
                    model.Age = patient.Age;
                    //model.Weight = patient.Weight;
                    model.Gender = patient.Gender;
                    //model.DateOfBirth = patient.DateOfBirth;
                    model.BloodGroupEnum = Utility.GetBloodGroup(patient.BloodGroup);
                    model.MobileNumber = patient.MobileNumber;
                    model.Address = patient.Address;
                }
            }
            return type == "appointMent" ? PartialView("EditorTemplates/_PatientsAppointment", model) : PartialView("EditorTemplates/_Patients", model);
        }
        public int GetNextRegNumber()
        {
            int nextRegNo = 1;
            if (_patientsRepository.GetAllActive().Result.Any())
            {
                nextRegNo = _patientsRepository.GetAllActive().Result.ToList().Max(c => c.RegNo) + 1;
            }
            return nextRegNo;
        }

        [HttpPost]
        public async Task<IActionResult> Save(PatientsViewModel model)
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
                    var patient = await _patientsRepository.Get(model.Id);
                    if (patient == null)
                    {
                        patient = new Patient();
                        patient.RegNo = GetNextRegNumber();
                    }
                    
                    patient.Name = model.Name;
                    patient.Age = model.Age;
                    //patient.Weight = model.Weight;
                    patient.Gender = model.Gender;
                    //patient.DateOfBirth = model.DateOfBirth;
                    patient.BloodGroup = model.BloodGroupEnum.ToString();
                    patient.MobileNumber = model.MobileNumber;
                    patient.Address = model.Address;
                    //patient.BirthDate = Utility.GetDateByValue(model.DateOfBirth);

                    patient.CreatedBy = user.Id;
                    patient.ModifiedBy = user.Id;

                    await _patientsRepository.Save(patient);
                    return RedirectToAction("Manage");
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator." + ex.Message.ToString());
                }
            }

            return View("EditorTemplates/_Patients", model);
        }

        [HttpPost]
        public async Task<JsonResult> SavePatient(PatientsViewModel model)
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
                    var patient = await _patientsRepository.Get(model.Id);
                    if (patient == null)
                    {
                        patient = new Patient();
                        patient.RegNo = GetNextRegNumber();
                    }

                    patient.Name = model.Name;
                    patient.Age = model.Age;
                    //patient.Weight = model.Weight;
                    patient.Gender = model.Gender;
                    //patient.DateOfBirth = model.DateOfBirth;
                    patient.BloodGroup = model.BloodGroupEnum.ToString();
                    patient.MobileNumber = model.MobileNumber;
                    patient.Address = model.Address;
                    //patient.BirthDate = Utility.GetDateByValue(model.DateOfBirth);

                    patient.CreatedBy = user.Id;
                    patient.ModifiedBy = user.Id;

                    await _patientsRepository.Save(patient);
                    return Json(patient.Id);
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator." + ex.Message.ToString());
                }
                //var user = await _userManager.GetUserAsync(User);
                //try
                //{
                //    var patient = await _patientsRepository.Get(model.Id);
                //    if (patient != null)
                //    {
                //        patient.Name = model.Name;
                //        patient.Age = model.Age;
                //        patient.Weight = model.Weight;
                //        patient.Gender = model.Gender;
                //        patient.DateOfBirth = model.DateOfBirth;
                //        patient.BloodGroup = model.BloodGroup.ToString();
                //        patient.MobileNumber = model.MobileNumber;
                //        patient.Address = model.Address;
                //        patient.Address = model.Address;

                //        patient.IsActive = true;
                //        patient.ModifiedDate = DateTime.Now;
                //        patient.ModifiedBy = user.Id;
                //        await _patientsRepository.Update(patient);
                //        return Json(patient.Id);
                //    }
                //    else
                //    {
                //        int nextRegNo = GetNextRegNumber();

                //        var aPatient = new Patient();
                //        aPatient.RegNo = nextRegNo;
                //        aPatient.Name = model.Name;
                //        aPatient.Age = model.Age;
                //        aPatient.Weight = model.Weight;
                //        aPatient.Gender = model.Gender;
                //        aPatient.DateOfBirth = model.DateOfBirth;
                //        aPatient.BloodGroup = model.BloodGroup.ToString();
                //        aPatient.MobileNumber = model.MobileNumber;
                //        aPatient.Address = model.Address;
                //        aPatient.Address = model.Address;

                //        aPatient.IsActive = true;
                //        aPatient.CreatedDate = DateTime.Now;
                //        aPatient.CreatedBy = user.Id;
                //        aPatient.ModifiedDate = DateTime.Now;
                //        aPatient.ModifiedBy = user.Id;

                //        await _patientsRepository.Insert(aPatient);
                //        return Json(aPatient.Id);
                //    }
                //}
                //catch (DbUpdateException ex)
                //{
                //    ModelState.AddModelError("", "Unable to save changes. " +
                //        "Try again, and if the problem persists " +
                //        "see your system administrator." + ex.Message.ToString());
                //}
            }

            return Json(model.Id);
        }

        [HttpGet]
        public async Task<JsonResult> PopulatePatientsDropDown(long patientId)
        {
            var patients = await _patientsRepository.GetAll();

            var models = patients.Where(x => !x.IsDeleted).ToList();

            var enumList = models.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return Json(enumList);
        }

        [HttpGet]
        public async Task<PartialViewResult> Delete(long id = 0, string type = null)
        {
            ObjectDeleteViewModel model = new ObjectDeleteViewModel();
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var patient = await _patientsRepository.Get(id);
                if (patient != null)
                {
                    model.Id = patient.Id;
                    model.Name = patient.Name;
                }
            }
            
            return type == "appointMent" ? PartialView("DisplayTemplates/_DeletePatientsAppointment", model) : PartialView("DisplayTemplates/_DeletePatients", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectDeleteViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Id.ToString()) && model.Id != 0)
            {
                var patient = await _patientsRepository.Get(model.Id);
                if (patient != null)
                {
                    patient.IsDeleted = true; ;
                    await _patientsRepository.Update(patient);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> DeletePatients(ObjectDeleteViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Id.ToString()) && model.Id != 0)
            {
                var patient = await _patientsRepository.Get(model.Id);
                if (patient != null)
                {
                    //var result = _patientsRepository.Delete(patient);
                    patient.IsDeleted = true; ;
                    await _patientsRepository.Update(patient);
                    //return RedirectToAction("Manage");
                    return Json(model.Id);
                }
            }
            return Json(model.Id);
        }
    }
}