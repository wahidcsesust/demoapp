using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Services;
using HealthCare.Data.Models;
using Microsoft.AspNetCore.Identity;
using HealthCare.Web.Models.Appointment;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.Data.Common;
using HealthCare.Web.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using HealthCare.Web.Models.PatientTests;
using HealthCare.Web.Models.PatientTestPayments;
using HealthCare.Data.Interfaces;
using HealthCare.Web.Services;
using System.Data.SqlClient;
using Dapper;

namespace HealthCare.Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<Patient> _patientsRepository;
        private readonly UserManager<User> _userManager;
        private readonly ICompositeViewEngine _viewEngine;

        private readonly IGenericRepository<Appointment> _appointmentRepository;
        private readonly IMedicalPaymentRepository _medicalPaymentRepository;
        private readonly IAccountHeadRepository _accountHeadRepository;
        public AppointmentController(
            IGenericRepository<Doctor> doctorRepository,
            IGenericRepository<Department> departmentRepository,
            IGenericRepository<Patient> patientsRepository,
            UserManager<User> userManager,
            ICompositeViewEngine viewEngine,
            IGenericRepository<Appointment> appointmentRepository,
            IMedicalPaymentRepository medicalPaymentRepository,
            IAccountHeadRepository accountHeadRepository
            )
        {
            _doctorRepository = doctorRepository;
            _departmentRepository = departmentRepository;
            _patientsRepository = patientsRepository;
            _userManager = userManager;
            _viewEngine = viewEngine;
            _appointmentRepository = appointmentRepository;
            _medicalPaymentRepository = medicalPaymentRepository;
            _accountHeadRepository = accountHeadRepository;
        }

        [HttpGet]
        public IActionResult Manage()
        {

            var query = $@"select app.*, pat.Name PatientName, doc.Name DoctorName from Appointments app
LEFT JOIN Patients pat on app.PatientId=pat.Id
LEFT JOIN Doctors doc on app.DoctorId=doc.Id
WHERE app.IsDeleted=0
ORDER BY app.Id DESC";
            var modelList = new List<AppointmentViewModel>();
            using (var con = new SqlConnection(Constants.ConnectionString))
            {
                modelList = con.Query<AppointmentViewModel>(query).ToList();
            }

            return View(modelList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            AppointmentViewModel model = new AppointmentViewModel() {
                AppointmentDateString = DateTime.Now.ToString(Constants.DateFormat)
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

            var departments = _departmentRepository.GetAll().Result.Where(d => !d.IsDeleted).ToList(); ;
            model.Departments = departments.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var appointment = await _appointmentRepository.Get(id);
                if (appointment != null)
                {
                    model.Id = appointment.Id;
                    model.SerialNo = appointment.SerialNo;
                    model.PatientId = appointment.PatientId;
                    model.DoctorId = appointment.DoctorId;
                    model.DepartmentId = appointment.DepartmentId;
                    //model.DateOfAppointment = appointment.DateOfAppointment;
                    model.AppointmentDateString = appointment.AppointmentDate != null ? appointment.AppointmentDate.Value.ToString(Constants.DateFormat) : string.Empty;
                    model.Problem = appointment.Problem;
                    model.Remarks = appointment.Remarks;
                    model.VisitAmount = appointment.VisitAmount;
                    if (appointment.DueAmount == null)
                    {
                        model.DueAmount = model.VisitAmount;
                    }
                    else
                    {
                        model.DueAmount = appointment.DueAmount ?? 0;
                    }
                    model.PaidAmount = 0;
                }
            }

            return View("EditorTemplates/Appointment", model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(AppointmentViewModel model)
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
                    var appointment = await _appointmentRepository.Get(model.Id);
                    if (appointment == null)
                    {
                        appointment = new Appointment();
                        appointment.SerialNo = GetNextSerialNumberByDoctorIdAppointDate(model.DoctorId, model.AppointmentDateString.StringToDate());
                        appointment.SequenceNo = GetNextSequenceNumber();
                        var patient = _patientsRepository.Get(model.PatientId);
                        appointment.AppointmentNo = Utility.GetAppointNo(appointment.SequenceNo, patient.Result.RegNo);
                    }
                    //appointment.SerialNo = model.SerialNo;
                    appointment.PatientId = model.PatientId;
                    appointment.DoctorId = model.DoctorId;
                    appointment.DepartmentId = model.DepartmentId;
                    appointment.AppointmentDate = model.AppointmentDateString.StringToDate();
                    appointment.Problem = model.Problem;
                    appointment.Remarks = model.Remarks;
                    appointment.VisitAmount = model.VisitAmount;

                    appointment.CreatedBy = user.Id;
                    appointment.ModifiedBy = user.Id;

                    await _appointmentRepository.Save(appointment);
                    //return RedirectToAction("Manage");
                    return RedirectToAction("CreateEdit", "Appointment", new { @id = appointment.Id});
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

            var departments = await _departmentRepository.GetAll();
            model.Departments = departments.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return View("EditorTemplates/Appointment", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var appointment = await _appointmentRepository.Get(id);
                if (appointment != null)
                {
                    name = appointment.SerialNo.ToString().PadLeft(3,'0');
                }
            }
            return PartialView("DisplayTemplates/_DeleteAppointment", name);
        }
        public int GetNextSequenceNumber()
        {
            int nextSequenceNo = 1;
            if (_appointmentRepository.GetAllActive().Result.Any())
            {
                nextSequenceNo = _appointmentRepository.GetAllActive().Result.ToList().Max(c => c.SequenceNo) + 1;
            }
            return nextSequenceNo;
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ObjectDeleteViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Id.ToString()) && model.Id != 0)
            {
                var appointment = await _appointmentRepository.Get(model.Id);
                if (appointment != null)
                {
                    appointment.IsDeleted = true;
                    await _appointmentRepository.Update(appointment);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PatientTest(long id = 0)
        {
            PatientTestViewModel model = new PatientTestViewModel();

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var appointment = await _appointmentRepository.Get(id);

                var doctor = await _doctorRepository.Get(appointment.DoctorId);
                var patient = await _patientsRepository.Get(appointment.PatientId);

                if (appointment != null)
                {
                    model.PatientId = appointment.PatientId;
                    model.DoctorId = appointment.DoctorId;
                    model.AppointmentId = appointment.Id;
                    model.PatientName = patient.Name ?? string.Empty;
                    model.DoctorName = doctor.Name ?? string.Empty;
                    model.PatientId = appointment.PatientId;
                }
            }

            return View("~/Views/PatientTest/EditorTemplates/PatientTest.cshtml", model);

            //AppointmentViewModel model = new AppointmentViewModel()
            //{
            //    DateOfAppointment = Utility.GetCurrentDateString()
            //};
            //var patients = _patientsRepository.GetAllActive().Result.ToList(); ;
            //model.Patients = patients.Select(x => new SelectListItem
            //{
            //    Text = x.Name,
            //    Value = x.Id.ToString()
            //}).ToList();

            //var doctors = _doctorRepository.GetAll().Result.Where(d => !d.IsDeleted).ToList();
            //model.Doctors = doctors.Select(x => new SelectListItem
            //{
            //    Text = x.Name,
            //    Value = x.Id.ToString()
            //}).ToList();

            //var departments = _departmentRepository.GetAll().Result.Where(d => !d.IsDeleted).ToList(); ;
            //model.Departments = departments.Select(x => new SelectListItem
            //{
            //    Text = x.Name,
            //    Value = x.Id.ToString()
            //}).ToList();

            //if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            //{
            //    var appointment = await _appointmentRepository.Get(id);
            //    if (appointment != null)
            //    {
            //        model.Id = appointment.Id;
            //        model.SerialNo = appointment.SerialNo;
            //        model.PatientId = appointment.PatientId;
            //        model.DoctorId = appointment.DoctorId;
            //        model.DepartmentId = appointment.DepartmentId;
            //        model.DateOfAppointment = appointment.DateOfAppointment;
            //        model.Problem = appointment.Problem;
            //        model.Remarks = appointment.Remarks;
            //        model.VisitAmount = appointment.VisitAmount;
            //        model.IsVisitPaymentDone = appointment.IsVisitPaymentDone;
            //    }
            //}
        }

        [HttpGet]
        public async Task<JsonResult> PopulateDoctorListDropDown(long departmentid)
        {
            var doctors = await _doctorRepository.GetAll();

            var models = doctors.Where(x => x.DepartmentId == departmentid).ToList();

            var enumList = models.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return Json(enumList);
        }

        [HttpGet]
        public async Task<JsonResult> GetDoctorSerialNumber(long doctorId, string appointmentDate)
        {
            int nextSerialNo = GetNextSerialNumberByDoctorIdAppointDate(doctorId, appointmentDate.StringToDate());

            var doctor = await _doctorRepository.Get(doctorId);

            var visit = doctor.VisitPrice;

            return Json(new { nextSerialNo = nextSerialNo.ToString().PadLeft(3, '0'), visit = visit });
        }

        public int GetNextSerialNumberByDoctorId(long doctorId)
        {
            int nextSerialNo = 1;
            if (_appointmentRepository.GetAll().Result.Where(c => c.DoctorId == doctorId && !c.IsDeleted).Any())
            {
                nextSerialNo = _appointmentRepository.GetAll().Result.ToList().Where(c => c.DoctorId == doctorId && !c.IsDeleted).Max(c => c.SerialNo) + 1;
            }
            return nextSerialNo;
        }
        public int GetNextSerialNumberByDoctorIdAppointDate(long doctorId, DateTime? appointDate)
        {
            int nextSerialNo = 1;

            if (_appointmentRepository.GetAll().Result.Where(c => c.DoctorId == doctorId && Convert.ToDateTime(c.AppointmentDate) == appointDate && !c.IsDeleted).Any())
            {
                nextSerialNo = _appointmentRepository.GetAll().Result.ToList().Where(c => c.DoctorId == doctorId && Convert.ToDateTime(c.AppointmentDate) == appointDate && !c.IsDeleted).Max(c => c.SerialNo) + 1;
            }
            return nextSerialNo;
        }
        [HttpGet]
        public async Task<JsonResult> PrintPrescription(long appointmentId = 0)
        {
            if (appointmentId > 0)
            {
                var appointment = await _appointmentRepository.Get(appointmentId);
                var doctor = await _doctorRepository.Get(appointment.DoctorId);
                var patient = await _patientsRepository.Get(appointment.PatientId);

                var department = _departmentRepository.Get(doctor.DepartmentId);

                ViewData["DoctorName"] = appointment != null ? (doctor.Name ?? string.Empty) : string.Empty;
                ViewData["DoctorDegree"] = appointment != null ? (doctor.DegreeOther ?? string.Empty) : string.Empty;
                ViewData["DoctorDepartment"] = department != null ? (department.Result.Name ?? string.Empty) : string.Empty;

                ViewData["PatientName"] = appointment != null ? (patient.Name ?? string.Empty) : string.Empty;
                ViewData["PatientAge"] = appointment != null ? (patient.Age.ToString() ?? string.Empty) : string.Empty;
                ViewData["PatientSex"] = appointment != null ? (patient.Gender ?? string.Empty) : string.Empty;
                ViewData["AppointmentDate"] = appointment.AppointmentDate != null ? appointment.AppointmentDate.Value.ToString(Constants.DateFormat) : string.Empty;
            }

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_Prescription", null);
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
        public async Task<JsonResult> CheckPaymentCollection(long appointmentId = 0, decimal totalAmount = 0, decimal paidAmount = 0, decimal dueAmount = 0)
        {
            var isResultOk = false;
            decimal paymentAmt = 0;
            string message = string.Empty;

            var model = await _appointmentRepository.Get(appointmentId);
            if (model != null)
            {
                if (model.DueAmount == null)
                {
                    paymentAmt = 0;
                }
                else
                {
                    paymentAmt = (model.VisitAmount ?? 0) - (model.DueAmount ?? 0);
                }

                if (totalAmount < (paidAmount + paymentAmt))
                {
                    isResultOk = true;
                    message = "Notifications : Please enter valid amount!";
                }
                else if(totalAmount == paymentAmt)
                {
                    isResultOk = true;
                    message = "Notifications : Payment already done. Thank You!";
                }
                else if(paidAmount == 0)
                {
                    isResultOk = true;
                    message = "Notifications : Please enter payment amount!";
                }

                //if(totalAmount < (paidAmount + (model.DueAmount ?? 0)) || (model.DueAmount <=paidAmount))
                //{
                //    isResultOk = true;
                //}
            }
            return Json(new { isResultOk = isResultOk, message = message });
        }
        [HttpGet]
        public async Task<IActionResult> SavePaymentCollection(long appointmentId = 0, decimal totalAmount = 0, decimal paidAmount = 0, decimal dueAmount = 0)
        {
            var isResultOk = false;
            var model = await _appointmentRepository.Get(appointmentId);
            decimal dueAmt = 0;
            decimal prePayAmt = 0;

            if (model != null)
            {
                if (model.DueAmount == null)
                {
                    dueAmt = totalAmount - paidAmount;
                }
                else
                {
                    prePayAmt = (model.VisitAmount ?? 0) - (model.DueAmount ?? 0);
                    dueAmt = (totalAmount - prePayAmt) - paidAmount;
                }

                model.VisitAmount = totalAmount;
                model.DueAmount = dueAmt;// totalAmount - paidAmount;
                await _appointmentRepository.Save(model);
                isResultOk = true;

                await _medicalPaymentRepository.UpdateMedicalPayment(model.Id, EnumTransactionType.DoctorVisit.ToString(), EnumPaymentType.Cash.ToString(), model.PatientId,
                    totalAmount, paidAmount, dueAmt, 0);

                await _accountHeadRepository.UpdateAccountHeadCurrentBalance(EnumTransactionType.DoctorVisit.ToString(), paidAmount);
            }
            return Json(new { isResultOk = isResultOk, dueAmount = dueAmt });
        }


        [HttpGet]
        public async Task<JsonResult> PrintInvoice(long appointmentId = 0)
        {
            if (appointmentId > 0)
            {
                var app = await _appointmentRepository.Get(appointmentId);

                var medicalPayment = await _medicalPaymentRepository.GetMedicalPaymentByTransactionId(appointmentId, EnumTransactionType.DoctorVisit.ToString());
                
                ViewData["InvoiceNumber"] = medicalPayment != null ? medicalPayment.InvoiceNo : string.Empty;
                ViewData["Amount"] = medicalPayment != null ? medicalPayment.TotalAmount.ToString() : string.Empty;
                ViewData["Date"] = Utility.GetCurrentDateString();
                ViewData["Particulars"] = "Doctor Visit Payment";
                ViewData["PatientName"] = app != null ? app.Patient.Name : string.Empty;
                ViewData["Age"] = app != null ? app.Patient.Age.ToString() : string.Empty;
            }

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_DoctorVisitInvoice", null);
            return Json(new { html = renderedView });
        }
    }
}