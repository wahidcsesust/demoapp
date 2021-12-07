using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Services;
using HealthCare.Data.Models;
using Microsoft.AspNetCore.Identity;
using HealthCare.Web.Models.Diagnosis;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace HealthCare.Web.Controllers
{
    public class DiagnosisController : Controller
    {
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<Patient> _patientsRepository;
        private readonly UserManager<User> _userManager;

        private readonly IGenericRepository<Diagnosis> _diagnosisRepository;
        public DiagnosisController(
            IGenericRepository<Doctor> doctorRepository,
            IGenericRepository<Department> departmentRepository,
            IGenericRepository<Patient> patientsRepository,
            UserManager<User> userManager,
            IGenericRepository<Diagnosis> diagnosisRepository
            )
        {
            _doctorRepository = doctorRepository;
            _departmentRepository = departmentRepository;
            _patientsRepository = patientsRepository;
            _userManager = userManager;
            _diagnosisRepository = diagnosisRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var objects = await _diagnosisRepository.GetAll();

            var diagnosiss = objects.Where(a => !a.IsDeleted);

            var viewModelList = new List<DiagnosisViewModel>();
            foreach (var diagnosis in diagnosiss)
            {
                var viewModel = new DiagnosisViewModel();
                viewModel.Id = diagnosis.Id;
                viewModel.DiagNo = diagnosis.DiagNo;

                var patient = new Patient();
                patient = await _patientsRepository.Get(diagnosis.PatientId);
                viewModel.PatientName = patient != null ? patient.Name : string.Empty;

                var doctor = new Doctor();
                doctor = await _doctorRepository.Get(diagnosis.DoctorId);
                viewModel.DoctorName = doctor != null ? doctor.Name : string.Empty;

                viewModel.DiagDate = diagnosis.DiagDate;
                viewModel.Description = diagnosis.Description;
                viewModel.Advice = diagnosis.Advice;
                viewModel.Remarks = diagnosis.Remarks;

                viewModelList.Add(viewModel);
            }

            return View(viewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            DiagnosisViewModel model = new DiagnosisViewModel();
            var patients = _patientsRepository.GetAll().Result.Where(p => !p.IsDeleted).ToList(); ;
            model.Patients = patients.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var doctors = _doctorRepository.GetAll().Result.Where(d => !d.IsDeleted).ToList();
            model.Doctors = doctors.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var diagnosis = await _diagnosisRepository.Get(id);
                if (diagnosis != null)
                {
                    model.DiagNo = diagnosis.DiagNo;
                    model.PatientId = diagnosis.PatientId;
                    model.DoctorId = diagnosis.DoctorId;
                    model.DiagDate = diagnosis.DiagDate;
                    model.Description = diagnosis.Description;
                    model.Advice = diagnosis.Advice;
                    model.Remarks = diagnosis.Remarks;
                }
            }

            return View("EditorTemplates/Diagnosis", model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(DiagnosisViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    var diagnosis = await _diagnosisRepository.Get(model.Id);
                    if (diagnosis != null)
                    {
                        diagnosis.DiagNo = model.DiagNo;
                        diagnosis.PatientId = model.PatientId;
                        diagnosis.DoctorId = model.DoctorId;
                        diagnosis.DiagDate = model.DiagDate;
                        diagnosis.Description = model.Description;
                        diagnosis.Advice = model.Advice;
                        diagnosis.Remarks = model.Remarks;

                        diagnosis.IsActive = true;
                        diagnosis.ModifiedDate = DateTime.Now;
                        diagnosis.ModifiedBy = user.Id;
                        await _diagnosisRepository.Update(diagnosis);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        int nextDiagNo = GetNextSerialNumberByDoctorId();// GetNextSerialNumberByDoctorId(model.DoctorId);

                        var aDiagnosis = new Diagnosis();
                        aDiagnosis.DiagNo = nextDiagNo;
                        aDiagnosis.PatientId = model.PatientId;
                        aDiagnosis.DoctorId = model.DoctorId;
                        aDiagnosis.DiagDate = model.DiagDate;
                        aDiagnosis.Description = model.Description;
                        aDiagnosis.Advice = model.Advice;
                        aDiagnosis.Remarks = model.Remarks;

                        aDiagnosis.IsActive = true;
                        aDiagnosis.CreatedDate = DateTime.Now;
                        aDiagnosis.CreatedBy = user.Id;
                        aDiagnosis.ModifiedDate = DateTime.Now;
                        aDiagnosis.ModifiedBy = user.Id;

                        await _diagnosisRepository.Insert(aDiagnosis);
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
            var patients = await _patientsRepository.GetAll();
            model.Patients = patients.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var doctors = await _doctorRepository.GetAll();
            model.Doctors = doctors.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            
            return View("EditorTemplates/Diagnosis", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var model = await _diagnosisRepository.Get(id);
                if (model != null)
                {
                    name = model.Description;
                }
            }
            return PartialView("DisplayTemplates/_DeleteDiagnosis", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id, FormCollection form)
        {
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var model = await _diagnosisRepository.Get(id);
                if (model != null)
                {
                    //var result = _doctorRepository.Delete(doctor);
                    model.IsDeleted = true; ;
                    await _diagnosisRepository.Update(model);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
        
        [HttpGet]
        public JsonResult GetDoctorDiagNumber(long doctorId)
        {
            int nextSerialNo = GetNextSerialNumberByDoctorId();// GetNextSerialNumberByDoctorId(doctorId);

            return Json(nextSerialNo.ToString().PadLeft(3, '0'));
        }

        public int GetNextSerialNumberByDoctorId(long doctorId)
        {
            int nextSerialNo = 1;
            if (_diagnosisRepository.GetAll().Result.Where(c => c.DoctorId == doctorId && !c.IsDeleted).Any())
            {
                nextSerialNo = _diagnosisRepository.GetAll().Result.ToList().Where(c => c.DoctorId == doctorId && !c.IsDeleted).Max(c => c.DiagNo) + 1;
            }
            return nextSerialNo;
        }

        public int GetNextSerialNumberByDoctorId()
        {
            int nextSerialNo = 1;
            if (_diagnosisRepository.GetAll().Result.Where(c => !c.IsDeleted).Any())
            {
                nextSerialNo = _diagnosisRepository.GetAll().Result.ToList().Where(c => !c.IsDeleted).Max(c => c.DiagNo) + 1;
            }
            return nextSerialNo;
        }
    }
}