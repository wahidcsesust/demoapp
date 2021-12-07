using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Services;
using HealthCare.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using HealthCare.Web.Models.PatientTests;
using HealthCare.Data.Common;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HealthCare.Data.Interfaces;
using HealthCare.Web.Models.PatientTestPayments;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using HealthCare.Web.Models;
using System.Data.SqlClient;
using Dapper;
using HealthCare.Web.Services;
using HealthCare.Web.Models.Admission;

namespace HealthCare.Web.Controllers
{
    public class AdmissionController : Controller
    {
        private readonly IGenericRepository<PatientAdmission> _patientAdmissionRepository;
        private readonly IGenericRepository<Bed> _bedRepository;
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<Patient> _patientsRepository;
        private readonly IGenericRepository<Appointment> _appointmentRepository;
        private readonly UserManager<User> _userManager;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IGenericRepository<MedicalTest> _medicalTestRepository;
        private readonly IPatientTestRepository _patientTestRepository;
        private readonly IMedicalPaymentRepository _medicalPaymentRepository;
        private readonly IGenericRepository<PatientTestDetail> _patientTestDetailRepository;
        private readonly IAccountHeadRepository _accountHeadRepository;
        public AdmissionController(
            IGenericRepository<PatientAdmission> patientAdmissionRepository,
            IGenericRepository<Bed> bedRepository,
            IGenericRepository<Doctor> doctorRepository,
            IGenericRepository<Department> departmentRepository,
            IGenericRepository<Patient> patientsRepository,
            UserManager<User> userManager,
            ICompositeViewEngine viewEngine,
            IGenericRepository<Appointment> appointmentRepository,
            IGenericRepository<MedicalTest> medicalTestRepository,
            IMedicalPaymentRepository medicalPaymentRepository,
            IPatientTestRepository patientTestRepository,
            IGenericRepository<PatientTestDetail> patientTestDetailRepository,
            IAccountHeadRepository accountHeadRepository
            )
        {
            _patientAdmissionRepository = patientAdmissionRepository;
            _bedRepository = bedRepository;
            _doctorRepository = doctorRepository;
            _departmentRepository = departmentRepository;
            _patientsRepository = patientsRepository;
            _userManager = userManager;
            _viewEngine = viewEngine;
            _appointmentRepository = appointmentRepository;
            _medicalTestRepository = medicalTestRepository;
            _medicalPaymentRepository = medicalPaymentRepository;
            _patientTestRepository = patientTestRepository;
            _patientTestDetailRepository = patientTestDetailRepository;
            _accountHeadRepository = accountHeadRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var admissions = await _patientAdmissionRepository.GetAllActive();

            var viewModelList = new List<AdmissionViewModel>();
            foreach (var model in admissions)
            {
                var viewModel = new AdmissionViewModel();
                viewModel.Id = model.Id;
                viewModel.PatientName = model.Patient.Name;
                viewModel.AdmissionDateString = model.AdmissionDate != null ? model.AdmissionDate.Value.ToString(Constants.DateFormat) : "";
                viewModel.AdmissionTime = model.AdmissionTime;
                viewModel.BedNo = (model.BedId != null && model.BedId != 0) ? model.Bed.BedNo : string.Empty;
                viewModelList.Add(viewModel);
            }

            return View(viewModelList);
        }
        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            AdmissionViewModel model = new AdmissionViewModel()
            {
                AdmissionDateString = DateTime.Now.ToString(Constants.DateFormat)  
            };

            var patients = _patientsRepository.GetAllActive().Result.ToList();
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

            var beds = _bedRepository.GetAllActive().Result.ToList();
            model.Beds = beds.Select(x => new SelectListItem
            {
                Text = x.BedNo,
                Value = x.Id.ToString()
            }).ToList();

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var admission = await _patientAdmissionRepository.Get(id);

                //var doctor = await _doctorRepository.Get(admission.DoctorId ?? 0);
                var patient = await _patientsRepository.Get(admission.PatientId);

                if (admission != null)
                {
                    model.Id = admission.Id;
                    model.PatientId = admission.PatientId;
                    model.DoctorId = admission.DoctorId;
                    model.BedId = admission.BedId;
                    model.AdmissionTime = admission.AdmissionTime;
                    model.CareOf = admission.CareOf;
                    model.AdmissionDateString = admission.AdmissionDate != null ? admission.AdmissionDate.Value.ToString(Constants.DateFormat) : "";

                    model.DischargeDateString = admission.DischargeDate != null ? admission.DischargeDate.Value.ToString(Constants.DateFormat) : "";
                    model.DischargeTime = admission.DischargeTime;
                    model.Remarks = admission.Remarks;

                    model.OperationRegNo = admission.OperationRegNo;
                    model.OperationDateString = admission.OperationDate != null ? admission.OperationDate.Value.ToString(Constants.DateFormat) : "";
                    model.OperationTime = admission.OperationTime;
                    model.OperationName = admission.OperationName;
                    model.Indication = admission.Indication;
                    model.Incision = admission.Incision;
                    model.Findings = admission.Findings;


                    model.Sex = admission.Sex;
                    model.Weight = admission.Weight;
                    model.ApgarScore = admission.ApgarScore;
                    model.Others = admission.Others;

                    //if (admission.DueAmount == null)
                    //{
                    //    model.DueAmount = model.TotalAmount;
                    //}
                    //else
                    //{
                    //    model.DueAmount = admission.DueAmount ?? 0;
                    //}
                    model.DueAmount = admission.DueAmount ?? 0;
                    model.PaidAmount = 0;
                    model.TotalAmount = admission.TotalAmount ?? 0;
                    model.LessAmount = admission.LessAmount ?? 0;
                }
            }

            return View("EditorTemplates/Admission", model);
        }
        [HttpPost]
        public async Task<IActionResult> Save(AdmissionViewModel model)
        {
            var errors = ModelState
                     .Where(x => x.Value.Errors.Count > 0)
                     .Select(x => new { x.Key, x.Value.Errors })
                     .ToArray();
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userManager.GetUserAsync(User).Result;
                    var _model = await _patientAdmissionRepository.Get(model.Id);
                    if(_model == null)
                    {
                        _model = new PatientAdmission();
                    }
                    _model.PatientId = model.PatientId;
                    _model.DoctorId = model.DoctorId;
                    _model.AdmissionDate = model.AdmissionDateString.StringToDate();
                    _model.AdmissionTime = model.AdmissionTime;
                    _model.RoomId = 1;
                    _model.BedId = model.BedId;
                    _model.CareOf = model.CareOf;
                    _model.CreatedBy = user.Id;
                    _model.ModifiedBy = user.Id;

                    await _patientAdmissionRepository.Save(_model);
                    return RedirectToAction("CreateEdit", "Admission", new { @id = _model.Id });
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator." + ex.Message.ToString());
                }
            }

            return View("EditorTemplates/Admission", model);
        }
        [HttpGet]
        public async Task<JsonResult> PrintAdmissionInvoice(long admissionId = 0)
        {
            if (admissionId > 0)
            {
                var admission = await _patientAdmissionRepository.Get(admissionId);


                var medicalPayment = await _medicalPaymentRepository.GetMedicalPaymentByTransactionId(admissionId, EnumTransactionType.Admission.ToString());
                var medicalPaymentDetail = medicalPayment.MedicalPaymentDetails.ToList();

                ViewData["InvoiceNumber"] = medicalPayment != null ? medicalPayment.InvoiceNo : string.Empty;

                ViewData["TotalAmount"] = admission != null ? admission.TotalAmount.ToString() : string.Empty;
                ViewData["LessAmount"] = admission != null ? admission.LessAmount.ToString() : string.Empty;
                ViewData["TotalCollection"] = medicalPaymentDetail.Select(a => a.Amount).Sum();
                ViewData["DueAmount"] = medicalPayment != null ? medicalPayment.DueAmount.ToString() : string.Empty;
                ViewData["Date"] = DateTime.Now.ToString(Constants.DateFormat); //(admission != null && admission.DischargeDate.HasValue) ? admission.DischargeDate.Value.ToString(Constants.DateFormat) : string.Empty;
                ViewData["PatientName"] = admission != null ? admission.Patient.Name : string.Empty;
                ViewData["Age"] = admission != null ? admission.Patient.Age.ToString() : string.Empty;
                ViewData["Address"] = admission != null ? admission.Patient.Address : string.Empty;
            }

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_AdmissionInvoice", null);
            return Json(new { html = renderedView });
        }
        private async Task<string> RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult =
                    _viewEngine.FindView(ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
        [HttpGet]
        public async Task<IActionResult> SaveAdmissionPayment(long admissionId = 0, decimal totalAmount = 0, decimal lessAmount = 0, decimal dueAmount = 0, decimal paidAmount = 0)
        {
            var isResultOk = false;
            var model = await _patientAdmissionRepository.Get(admissionId);

            if (model != null)
            {
                model.TotalAmount = totalAmount;
                model.LessAmount = lessAmount;
                model.DueAmount = dueAmount;
                await _patientAdmissionRepository.Save(model);
                isResultOk = true;

                await _medicalPaymentRepository.UpdateMedicalPayment(model.Id, EnumTransactionType.Admission.ToString(), EnumPaymentType.Cash.ToString(), model.PatientId,
                    totalAmount, paidAmount, dueAmount, lessAmount);

                await _accountHeadRepository.UpdateAccountHeadCurrentBalance(EnumTransactionType.Admission.ToString(), paidAmount);
            }
            return Json(new { isResultOk = isResultOk });
        }
        [HttpGet]
        public async Task<IActionResult> SaveOperation(long admissionId, string operationRegNo, string operationDateString, string operationTime,
            string operationName, string indication, string incision, string findings)
        {
            var isResultOk = false;
            var model = await _patientAdmissionRepository.Get(admissionId);

            if (model != null)
            {
                model.OperationRegNo = operationRegNo;
                model.OperationDate = operationDateString.StringToDate();
                model.OperationTime = operationTime;
                model.OperationName = operationName;
                model.Indication = indication;
                model.Incision = incision;
                model.Findings = findings;
                await _patientAdmissionRepository.Save(model);
                isResultOk = true;
            }
            return Json(new { isResultOk = isResultOk });
        }
        [HttpGet]
        public async Task<IActionResult> SaveBabyNote(long admissionId, string sex, int? weight, string apgarScore, string others)
        {
            var isResultOk = false;
            var model = await _patientAdmissionRepository.Get(admissionId);

            if (model != null)
            {
                model.Sex = sex;
                model.Weight = weight;
                model.ApgarScore = apgarScore;
                model.Others = others;
                await _patientAdmissionRepository.Save(model);
                isResultOk = true;
            }
            return Json(new { isResultOk = isResultOk });
        }
        [HttpGet]
        public async Task<IActionResult> SaveDischarge(long admissionId, string dischargeDateString, string dischargeTime, string remarks)
        {
            var isResultOk = false;
            var model = await _patientAdmissionRepository.Get(admissionId);

            if (model != null)
            {
                model.DischargeDate = dischargeDateString.StringToDate();
                model.DischargeTime = dischargeTime;
                model.Remarks = remarks;
                await _patientAdmissionRepository.Save(model);
                isResultOk = true;
            }
            return Json(new { isResultOk = isResultOk });
        }

        //[HttpPost]
        //public async Task<IActionResult> SaveDischarge(AdmissionViewModel model)
        //{
        //    var errors = ModelState
        //             .Where(x => x.Value.Errors.Count > 0)
        //             .Select(x => new { x.Key, x.Value.Errors })
        //             .ToArray();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var user = _userManager.GetUserAsync(User).Result;
        //            var _model = await _patientAdmissionRepository.Get(model.Id);
        //            _model.DischargeDate = model.DischargeDateString.StringToDate();
        //            _model.Remarks = model.Remarks;

        //            await _patientAdmissionRepository.Save(_model);
        //            return RedirectToAction("CreateEdit", "Admission", new { @id = _model.Id });
        //        }
        //        catch (DbUpdateException ex)
        //        {
        //            //Log the error (uncomment ex variable name and write a log.
        //            ModelState.AddModelError("", "Unable to save changes. " +
        //                "Try again, and if the problem persists " +
        //                "see your system administrator." + ex.Message.ToString());
        //        }
        //    }

        //    return View("EditorTemplates/Admission", model);
        //}
    }
}