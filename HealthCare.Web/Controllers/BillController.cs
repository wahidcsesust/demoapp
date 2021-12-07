using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Services;
using HealthCare.Data.Models;
using Microsoft.AspNetCore.Identity;
using HealthCare.Web.Models.Bill;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace HealthCare.Web.Controllers
{
    public class BillController : Controller
    {
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<Patient> _patientsRepository;
        private readonly UserManager<User> _userManager;

        private readonly IGenericRepository<Bill> _billRepository;
        public BillController(
            IGenericRepository<Doctor> doctorRepository,
            IGenericRepository<Department> departmentRepository,
            IGenericRepository<Patient> patientsRepository,
            UserManager<User> userManager,
            IGenericRepository<Bill> billRepository
            )
        {
            _doctorRepository = doctorRepository;
            _departmentRepository = departmentRepository;
            _patientsRepository = patientsRepository;
            _userManager = userManager;
            _billRepository = billRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var objects = await _billRepository.GetAll();

            var models = objects.Where(a => !a.IsDeleted);

            var viewModelList = new List<BillViewModel>();
            foreach (var model in models)
            {
                var viewModel = new BillViewModel();
                viewModel.Id = model.Id;
                viewModel.BillNo = model.BillNo;

                var patient = new Patient();
                patient = await _patientsRepository.Get(model.PatientId);
                viewModel.PatientName = patient != null ? patient.Name : string.Empty;

                var doctor = new Doctor();
                doctor = await _doctorRepository.Get(model.DoctorId);
                viewModel.DoctorName = doctor != null ? doctor.Name : string.Empty;

                viewModel.BillDate = model.BillDate;
                viewModel.Amount = model.Amount;
                viewModel.Remarks = model.Remarks;

                viewModelList.Add(viewModel);
            }

            return View(viewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            BillViewModel viewModel = new BillViewModel();
            var patients = _patientsRepository.GetAll().Result.Where(p => !p.IsDeleted).ToList(); ;
            viewModel.Patients = patients.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var doctors = _doctorRepository.GetAll().Result.Where(d => !d.IsDeleted).ToList();
            viewModel.Doctors = doctors.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var model = await _billRepository.Get(id);
                if (model != null)
                {
                    viewModel.BillNo = model.BillNo;
                    viewModel.PatientId = model.PatientId;
                    viewModel.DoctorId = model.DoctorId;
                    viewModel.BillDate = model.BillDate;
                    viewModel.Amount = model.Amount;
                    viewModel.Remarks = model.Remarks;
                }
            }

            return View("EditorTemplates/Bill", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save(BillViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    var model = await _billRepository.Get(viewModel.Id);
                    if (model != null)
                    {
                        model.PatientId = viewModel.PatientId;
                        model.DoctorId = viewModel.DoctorId;
                        model.BillDate = viewModel.BillDate;
                        model.Amount = viewModel.Amount;
                        model.Remarks = viewModel.Remarks;

                        model.IsActive = true;
                        model.ModifiedDate = DateTime.Now;
                        model.ModifiedBy = user.Id;
                        await _billRepository.Update(model);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        int nextBillNo = GetNextBillNumber();

                        var aDiagnosis = new Bill();
                        aDiagnosis.BillNo = nextBillNo;
                        aDiagnosis.PatientId = viewModel.PatientId;
                        aDiagnosis.DoctorId = viewModel.DoctorId;
                        aDiagnosis.BillDate = viewModel.BillDate;
                        aDiagnosis.Amount = viewModel.Amount;
                        aDiagnosis.Remarks = viewModel.Remarks;

                        aDiagnosis.IsActive = true;
                        aDiagnosis.CreatedDate = DateTime.Now;
                        aDiagnosis.CreatedBy = user.Id;
                        aDiagnosis.ModifiedDate = DateTime.Now;
                        aDiagnosis.ModifiedBy = user.Id;

                        await _billRepository.Insert(aDiagnosis);
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
            viewModel.Patients = patients.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var doctors = await _doctorRepository.GetAll();
            viewModel.Doctors = doctors.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return View("EditorTemplates/Bill", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var model = await _billRepository.Get(id);
                if (model != null)
                {
                    name = model.BillNo.ToString();
                }
            }
            return PartialView("DisplayTemplates/_DeleteBill", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id, FormCollection form)
        {
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var model = await _billRepository.Get(id);
                if (model != null)
                {
                    //var result = _doctorRepository.Delete(doctor);
                    model.IsDeleted = true; ;
                    await _billRepository.Update(model);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetBillNumber(long doctorId)
        {
            int nextSerialNo = GetNextBillNumber();
            int amount = 0;

            var model = _doctorRepository.Get(doctorId).Result;
            if(model != null)
            {
                amount = model.VisitPrice;
            }

            return Json(new { billNo = "B-" + nextSerialNo.ToString().PadLeft(5, '0'), amount = amount });
        }

        public int GetNextBillNumber()
        {
            int nextSerialNo = 1;
            if (_billRepository.GetAll().Result.Where(c => !c.IsDeleted).Any())
            {
                nextSerialNo = _billRepository.GetAll().Result.ToList().Where(c => !c.IsDeleted).Max(c => c.BillNo) + 1;
            }
            return nextSerialNo;
        }
    }
}