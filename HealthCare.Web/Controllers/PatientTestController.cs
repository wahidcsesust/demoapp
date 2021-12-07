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

namespace HealthCare.Web.Controllers
{
    public class PatientTestController : Controller
    {
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
        public PatientTestController(
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
        public IActionResult Manage()
        {
            var query = $@"select pt.*, pat.Name PatientName, doc.Name DoctorName, ISNULL(#ptd.NoOfMedicalTests, 0) NoOfMedicalTests, CASE WHEN pt.AppointmentId = 0 THEN 'No Appointment' ELSE app.AppointmentNo END AppointmentNo
from PatientTests pt
LEFT JOIN
(
	select PatientTestId, count(Id) NoOfMedicalTests from PatientTestDetails WHERE IsDeleted=0 GROUP BY PatientTestId
) #ptd on #ptd.PatientTestId = pt.Id
LEFT JOIN Patients pat on pat.Id=pt.PatientId
LEFT JOIN Doctors doc on doc.Id=pt.DoctorId
LEFT JOIN Appointments app on app.Id=pt.AppointmentId
WHERE pt.IsDeleted=0 ORDER BY pt.Id DESC";
            var modelList = new List<PatientTestViewModel>();
            using (var con = new SqlConnection(Constants.ConnectionString))
            {
                modelList = con.Query<PatientTestViewModel>(query).ToList();
            }

            return View(modelList);

            //var patientTests = await _patientTestRepository.GetAllActive();

            //var list = patientTests.OrderByDescending(d => d.Id).ToList();

            //var viewModelList = new List<PatientTestViewModel>();
            //foreach (var patientTest in list)
            //{
            //    var viewModel = new PatientTestViewModel();
            //    viewModel.Id = patientTest.Id;
            //    viewModel.AppointmentId = patientTest.AppointmentId;
            //    viewModel.AppointmentNo = patientTest.AppointmentId == 0 ? "No Appointment" : patientTest.Appointment.AppointmentNo;
            //    viewModel.PatientName = patientTest.Patient.Name;
            //    viewModel.DoctorName = patientTest.Doctor.Name;
            //    viewModel.DeliveryDate = patientTest.DeliveryDate;
            //    viewModel.NoOfMedicalTests = patientTest.PatientTestDetails.LongCount();

            //    viewModelList.Add(viewModel);
            //}

            //return View(viewModelList);
        }
        [HttpGet]
        public async Task<IActionResult> CreateEditAddHoc(long id = 0)
        {
            PatientTestViewModel model = new PatientTestViewModel()
            {
                DeliveryDateString = DateTime.Now.ToString(Constants.DateFormat)
            };

            var appointments = await _appointmentRepository.GetAllActive();
            model.Appointments = appointments.Select(a => new SelectListItem
            {
                Text = a.AppointmentNo,
                Value = a.Id.ToString()
            }).ToList();

            model.Appointments = new List<SelectListItem>();

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

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var patientTest = await _patientTestRepository.Get(id);

                var doctor = await _doctorRepository.Get(patientTest.DoctorId);
                var patient = await _patientsRepository.Get(patientTest.PatientId);

                if (patientTest != null)
                {
                    model.Id = patientTest.Id;
                    model.PatientId = patientTest.PatientId;
                    model.DoctorId = patientTest.DoctorId;
                    model.AppointmentId = patientTest.AppointmentId;
                    model.DeliveryDateString = patientTest.DeliveryDate != null ? patientTest.DeliveryDate.Value.ToString(Constants.DateFormat) : string.Empty;
                    //model.RegNo = Utility.RegNoPadding(patientTest.Patient.RegNo);
                    //model.AppointmentNo = patientTest.Appointment.AppointmentNo;
                    //model.PatientName = patientTest.Patient.Name ?? string.Empty;
                    //model.DoctorName = patientTest.Doctor.Name ?? string.Empty;

                    //model.DateOfDelivery = patientTest.DateOfDelivery;

                    var medicalTests = await _medicalTestRepository.GetAllActive();
                    var ptList = new List<PatientTestDetail>();
                    foreach (var pt in patientTest.PatientTestDetails)
                    {
                        var ptModel = new PatientTestDetail
                        {
                            Id = pt.Id,
                            MedicalTestId = pt.MedicalTestId,
                            PatientTestId = pt.PatientTestId,
                            TestRate = pt.TestRate,
                            Discount = pt.Discount,
                            Amount = pt.Amount,
                            MedicalTests = medicalTests.Select(x => new SelectListItem
                            {
                                Text = x.Name,
                                Value = x.Id.ToString()
                            }).ToList()
                        };
                        ptList.Add(ptModel);
                    }
                    model.PatientTestDetails = ptList;

                    if (patientTest.PatientTestDetails.Count > 0)
                    {
                        model.TotalAmount = patientTest.PatientTestDetails.Select(a => a.Amount).Sum();

                        if (patientTest.DueAmount == null)
                        {
                            model.DueAmount = model.TotalAmount;
                        }
                        else
                        {
                            model.DueAmount = patientTest.DueAmount ?? 0;
                        }
                        model.PaidAmount = 0;
                        model.LessAmount = patientTest.LessAmount ?? 0;
                    }
                }
            }

            return View("EditorTemplates/PatientTestAddHoc", model);
        }
        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            PatientTestViewModel model = new PatientTestViewModel()
            {
                DeliveryDateString = DateTime.Now.ToString(Constants.DateFormat)
            };

            var appointments = _appointmentRepository.GetAllActive().Result;
            model.Appointments = appointments.Select(a => new SelectListItem
            {
                Text = a.AppointmentNo,
                Value = a.Id.ToString()
            }).ToList();

            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var patientTest = await _patientTestRepository.Get(id);

                var doctor = await _doctorRepository.Get(patientTest.DoctorId);
                var patient = await _patientsRepository.Get(patientTest.PatientId);

                if (patientTest != null)
                {
                    model.Id = patientTest.Id;
                    model.PatientId = patientTest.PatientId;
                    model.DoctorId = patientTest.DoctorId;
                    model.AppointmentId = patientTest.AppointmentId;

                    model.RegNo = Utility.RegNoPadding(patientTest.Patient.RegNo);
                    model.AppointmentNo = patientTest.Appointment.AppointmentNo;
                    model.PatientName = patientTest.Patient.Name ?? string.Empty;
                    model.DoctorName = patientTest.Doctor.Name ?? string.Empty;
                    model.DeliveryDateString = patientTest.DeliveryDate != null ? patientTest.DeliveryDate.Value.ToString(Constants.DateFormat) : string.Empty;
                    //model.DateOfDelivery = patientTest.DateOfDelivery;

                    var medicalTests = await _medicalTestRepository.GetAllActive();
                    var ptList = new List<PatientTestDetail>();
                    foreach (var pt in patientTest.PatientTestDetails)
                    {
                        var ptModel = new PatientTestDetail
                        {
                            Id = pt.Id,
                            MedicalTestId = pt.MedicalTestId,
                            PatientTestId = pt.PatientTestId,
                            TestRate = pt.TestRate,
                            Discount = pt.Discount,
                            Amount = pt.Amount,
                            MedicalTests = medicalTests.Select(x => new SelectListItem
                            {
                                Text = x.Name,
                                Value = x.Id.ToString()
                            }).ToList()
                        };
                        ptList.Add(ptModel);
                    }
                    model.PatientTestDetails = ptList;

                    if (patientTest.PatientTestDetails.Count > 0)
                    {
                        model.TotalAmount = patientTest.PatientTestDetails.Select(a => a.Amount).Sum();

                        if (patientTest.DueAmount == null)
                        {
                            model.DueAmount = model.TotalAmount;
                        }
                        else
                        {
                            model.DueAmount = patientTest.DueAmount ?? 0;
                        }
                        model.PaidAmount = 0;
                        model.LessAmount = patientTest.LessAmount ?? 0;
                    }
                }
            }

            return View("EditorTemplates/PatientTest", model);
        }
        public IActionResult IndexVC()
        {
            return ViewComponent("CreateEditPatientTestDetail", new { maxPriority = 3, isDone = false });
        }
        public async Task<IActionResult> CreateEditPatientTestDetail(long parentId, long childId)
        {
            var model = new PatientTestDetail();
            var medicalTests = await _medicalTestRepository.GetAllActive();
            model.MedicalTests = medicalTests.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return PartialView("EditorTemplates/_PatientTestDetail", model);
        }
        public async Task<JsonResult> DeletePatientTestItem(long parentId, long childId = 0)
        {

            var isResultOk = false;
            string message = string.Empty;

            var _model = await _medicalPaymentRepository.GetMedicalPaymentByTransactionId(parentId, EnumTransactionType.MedicalTest.ToString());
            if (_model != null)
            {
                isResultOk = false;
                message = "Notifications : Payment collection already started. You cannot delete test item!";
                return Json(new { isResultOk = isResultOk, message = message });
            }

            var model = await _patientTestDetailRepository.Get(childId);
            if (model != null)
            {
                model.IsDeleted = true;
                isResultOk = true;
                await _patientTestDetailRepository.Delete(model);
                //await _accountHeadRepository.DeleteAccountHeadCurrentBalance(EnumTransactionType.MedicalTest.ToString(), model.Amount ?? 0);
            }
            return Json(new { isResultOk = isResultOk, message = message });
        }

        [HttpPost]
        public async Task<IActionResult> Save(PatientTestViewModel model)
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
                    var patientTest = await _patientTestRepository.Get(model.Id);
                    if (patientTest == null)
                    {
                        patientTest = new PatientTest();
                    }
                    patientTest.PatientId = model.PatientId;
                    patientTest.DoctorId = model.DoctorId;
                    patientTest.AppointmentId = model.AppointmentId;
                    //patientTest.TotalAmount = model.TotalAmount;
                    //patientTest.DueAmount = model.DueAmount;
                    //patientTest.DateOfDelivery = model.DateOfDelivery;
                    patientTest.DeliveryDate = model.DeliveryDateString.StringToDate();

                    patientTest.CreatedBy = user.Id;
                    patientTest.ModifiedBy = user.Id;

                    // Delete children
                    foreach (var patientTestDetail in patientTest.PatientTestDetails.ToList())
                    {
                        if (!model.PatientTestDetails.Any(c => c.Id == patientTestDetail.Id))
                        {
                            //patientTestDetail.IsDeleted = true;
                            await _patientTestDetailRepository.Delete(patientTestDetail);
                            model.PatientTestDetails.Remove(patientTestDetail);
                        }
                    }
                    // Update and Insert PatientTestDetails
                    if (model.PatientTestDetails.Count > 0)
                    {
                        patientTest.TotalAmount = model.PatientTestDetails.Select(a => a.Amount).Sum();
                        foreach (var ptDetail in model.PatientTestDetails)
                        {
                            var existingPtDetail = await _patientTestDetailRepository.Get(ptDetail.Id); //model.PatientTestDetails.Where(p => p.Id == ptDetail.Id).SingleOrDefault();

                            if (existingPtDetail == null)
                            {
                                existingPtDetail = new PatientTestDetail()
                                {
                                    IsActive = true,
                                    IsDeleted = false,
                                    IsLocked = false,
                                    CreatedBy = user.Id,
                                    CreatedDate = DateTime.Now,
                                    ModifiedBy = user.Id,
                                    ModifiedDate = DateTime.Now
                                };
                            }
                            existingPtDetail.PatientTestId = patientTest.Id;
                            existingPtDetail.MedicalTestId = ptDetail.MedicalTestId;
                            existingPtDetail.TestRate = ptDetail.TestRate;
                            existingPtDetail.Discount = ptDetail.Discount;
                            existingPtDetail.Amount = ptDetail.Amount;
                            patientTest.PatientTestDetails.Add(existingPtDetail);
                        }
                    }
                    await _patientTestRepository.Save(patientTest);
                    //return RedirectToAction("Manage");
                    return RedirectToAction("CreateEdit", "PatientTest", new { @id = patientTest.Id });
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator." + ex.Message.ToString());
                }
            }

            return View("EditorTemplates/PatientTest", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAddHoc(PatientTestViewModel model)
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
                    var patientTest = await _patientTestRepository.Get(model.Id);
                    if (patientTest == null)
                    {
                        patientTest = new PatientTest();
                    }
                    patientTest.PatientId = model.PatientId;
                    patientTest.DoctorId = model.DoctorId;
                    patientTest.AppointmentId = model.AppointmentId;
                    //patientTest.TotalAmount = model.TotalAmount;
                    //patientTest.DueAmount = model.DueAmount;
                    //patientTest.DateOfDelivery = model.DateOfDelivery;
                    patientTest.DeliveryDate = model.DeliveryDateString.StringToDate();

                    patientTest.CreatedBy = user.Id;
                    patientTest.ModifiedBy = user.Id;

                    // Delete children
                    foreach (var patientTestDetail in patientTest.PatientTestDetails.ToList())
                    {
                        if (!model.PatientTestDetails.Any(c => c.Id == patientTestDetail.Id))
                        {
                            //patientTestDetail.IsDeleted = true;
                            await _patientTestDetailRepository.Delete(patientTestDetail);
                            model.PatientTestDetails.Remove(patientTestDetail);
                        }
                    }
                    // Update and Insert PatientTestDetails
                    if (model.PatientTestDetails.Count > 0)
                    {
                        patientTest.TotalAmount = model.PatientTestDetails.Select(a => a.Amount).Sum();
                        foreach (var ptDetail in model.PatientTestDetails)
                        {
                            var existingPtDetail = await _patientTestDetailRepository.Get(ptDetail.Id); //model.PatientTestDetails.Where(p => p.Id == ptDetail.Id).SingleOrDefault();

                            if (existingPtDetail == null)
                            {
                                existingPtDetail = new PatientTestDetail()
                                {
                                    IsActive = true,
                                    IsDeleted = false,
                                    IsLocked = false,
                                    CreatedBy = user.Id,
                                    CreatedDate = DateTime.Now,
                                    ModifiedBy = user.Id,
                                    ModifiedDate = DateTime.Now
                                };
                            }
                            existingPtDetail.PatientTestId = patientTest.Id;
                            existingPtDetail.MedicalTestId = ptDetail.MedicalTestId;
                            existingPtDetail.TestRate = ptDetail.TestRate;
                            existingPtDetail.Discount = ptDetail.Discount;
                            existingPtDetail.Amount = ptDetail.Amount;
                            patientTest.PatientTestDetails.Add(existingPtDetail);
                        }
                    }
                    await _patientTestRepository.Save(patientTest);
                    //return RedirectToAction("Manage");
                    return RedirectToAction("CreateEditAddHoc", "PatientTest", new { @id = patientTest.Id });
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator." + ex.Message.ToString());
                }
            }

            return View("EditorTemplates/PatientTestAddHoc", model);
        }

        [HttpGet]
        public async Task<JsonResult> GetAppointmentDetail(long objectId)
        {
            var appointment = await _appointmentRepository.Get(objectId);

            var regNo = Utility.RegNoPadding(appointment.Patient.RegNo);
            var patientName = appointment.Patient.Name;
            var doctorName = appointment.Doctor.Name;
            var patientId = appointment.PatientId;
            var doctorId = appointment.DoctorId;

            return Json(new { regNo = regNo, patientName = patientName, doctorName = doctorName, patientId = patientId, doctorId = doctorId });
        }
        
        [HttpGet]
        public async Task<JsonResult> CheckPaymentStarted(long patientTestId = 0)
        {
            var isResultOk = false;
            string message = string.Empty;

            var model = await _medicalPaymentRepository.GetMedicalPaymentByTransactionId(patientTestId, EnumTransactionType.MedicalTest.ToString());
            if (model != null)
            {
                isResultOk = true;
                message = "Notifications : Payment collection already started. You cannot add new test!";
            }
            return Json(new { isResultOk = isResultOk, message = message });
        }

        [HttpGet]
        public async Task<JsonResult> CheckPaymentCollection(long patientTestId = 0, decimal totalAmount = 0, decimal paidAmount = 0, decimal dueAmount = 0, int discount = 0)
        {
            var isResultOk = false;
            decimal paymentAmt = 0;
            string message = string.Empty;

            var model = await _patientTestRepository.Get(patientTestId);
            if (model != null)
            {
                if (model.DueAmount == null)
                {
                    paymentAmt = 0;
                }
                else
                {
                    paymentAmt = (model.TotalAmount ?? 0) - (model.DueAmount ?? 0);
                }

                if (totalAmount < (paidAmount + paymentAmt))
                {
                    isResultOk = true;
                    message = "Notifications : Please enter valid amount!";
                }
                else if (totalAmount == paymentAmt)
                {
                    isResultOk = true;
                    message = "Notifications : Payment already done. Thank You!";
                }
                else if (paidAmount == 0)
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
        public async Task<JsonResult> CheckPatientTest(long appointmentId = 0)
        {
            var isResultOk = false;
            string message = string.Empty;

            var model = await _patientTestRepository.GetPatientTestByAppointmentId(appointmentId);
            if (model != null)
            {
                isResultOk = true;
                message = "Notifications : Appointment already exists!";
            }
            return Json(new { isResultOk = isResultOk, message = message });
        }
        
        [HttpGet]
        public async Task<IActionResult> SaveTestPayment(long patientTestId = 0, decimal totalAmount = 0, decimal lessAmount = 0, decimal dueAmount = 0, decimal paidAmount = 0)
        {
            var isResultOk = false;
            var model = await _patientTestRepository.Get(patientTestId);

            if (model != null)
            {
                model.LessAmount = lessAmount;
                model.DueAmount = dueAmount;
                await _patientTestRepository.Save(model);
                isResultOk = true;

                await _medicalPaymentRepository.UpdateMedicalPayment(model.Id, EnumTransactionType.MedicalTest.ToString(), EnumPaymentType.Cash.ToString(), model.PatientId,
                    totalAmount, paidAmount, dueAmount, lessAmount);

                await _accountHeadRepository.UpdateAccountHeadCurrentBalance(EnumTransactionType.MedicalTest.ToString(), paidAmount);
            }
            return Json(new { isResultOk = isResultOk });
        }
        [HttpGet]
        public async Task<JsonResult> PrintPatientTestInvoice(long patientTestId = 0)
        {
            if (patientTestId > 0)
            {
                var patientTest = await _patientTestRepository.Get(patientTestId);
                var patientTestDetail = patientTest.PatientTestDetails.ToList();

                var query = $@"SELECT c.Code,c.Name,b.Amount FROM PatientTests a
                                INNER JOIN PatientTestDetails b on b.PatientTestId=a.Id
                                INNER JOIN MedicalTests c on c.Id=b.MedicalTestId
                                WHERE a.Id={patientTestId} ORDER BY b.Id";
                var patientTestItemViewModel = new List<PatientTestInvoiceViewModel>();
                using (var con = new SqlConnection(Constants.ConnectionString))
                {
                    patientTestItemViewModel = con.Query<PatientTestInvoiceViewModel>(query).ToList();
                }

                ViewBag.PatientTestItem = patientTestItemViewModel;


                var medicalPayment = await _medicalPaymentRepository.GetMedicalPaymentByTransactionId(patientTestId, EnumTransactionType.MedicalTest.ToString());
                var medicalPaymentDetail = medicalPayment.MedicalPaymentDetails.ToList();

                ViewData["InvoiceNumber"] = medicalPayment != null ? medicalPayment.InvoiceNo : string.Empty;

                ViewData["TotalAmount"] = patientTest != null ? patientTest.TotalAmount.ToString() : string.Empty;
                ViewData["LessAmount"] = patientTest != null ? patientTest.LessAmount.ToString() : string.Empty;
                ViewData["TotalCollection"] = medicalPaymentDetail.Select(a => a.Amount).Sum();
                ViewData["DueAmount"] = medicalPayment != null ? medicalPayment.DueAmount.ToString() : string.Empty;
                //ViewData["Date"] = patientTest != null ? Utility.GetDateFormatByValue(patientTest.DateOfDelivery) : string.Empty;
                ViewData["Date"] = (patientTest != null && patientTest.DeliveryDate.HasValue) ? patientTest.DeliveryDate.Value.ToString(Constants.DateFormat) : string.Empty;
                ViewData["PatientName"] = patientTest != null ? patientTest.Patient.Name : string.Empty;
                ViewData["Age"] = patientTest != null ? patientTest.Patient.Age.ToString() : string.Empty;
                ViewData["Address"] = patientTest != null ? patientTest.Patient.Address : string.Empty;

                //var pTestDetailList = patientTestDetail.Select(b => new SelectListItem()
                //{
                //    Value = b.Id.ToString(),
                //    Text = b.MedicalTest.Name.ToString()
                //});
                //ViewBag.ListMedicalPaymentDetail = new SelectList(pTestDetailList, "Value", "Text");

                //var enumList = medicalPaymentDetail.Select(b => new SelectListItem()
                //{
                //    Value = b.Id.ToString(),
                //    Text = b.Amount.ToString()
                //});
                //ViewBag.ListMedicalPaymentDetail = new SelectList(enumList, "Value", "Text");

                //if (patientTest != null && (patientTest.LessAmount == null || patientTest.LessAmount.Value == 0))
                //{
                //    ViewData["TotalCollection"] = medicalPaymentDetail.Select(a => a.Amount).Sum();
                //    ViewData["InvoiceNumber"] = medicalPayment != null ? medicalPayment.InvoiceNo : string.Empty;
                //    ViewData["Amount"] = patientTest != null ? patientTest.TotalAmount.ToString() : string.Empty;
                //    ViewData["Discount"] = "0";
                //    ViewData["DiscountAmount"] = patientTest != null ? patientTest.TotalAmount.ToString() : string.Empty;
                //    ViewData["Due"] = medicalPayment != null ? medicalPayment.DueAmount.ToString() : string.Empty;
                //    ViewData["Date"] = patientTest != null ? Utility.GetDateFormatByValue(patientTest.DateOfDelivery) : string.Empty;
                //    ViewData["Particulars"] = "Medical Test Payment";
                //}
                //else
                //{
                //    var discount = patientTest.LessAmount.Value;
                //    decimal discountPercent = Convert.ToDecimal(discount) / 100;
                //    var discountTotal = patientTest.TotalAmount - (discountPercent * patientTest.TotalAmount);

                //    var round = Math.Round(discountTotal.Value);

                //    ViewData["TotalCollection"] = round.ToString(); //medicalPaymentDetail.Select(a => a.Amount).Sum();
                //    ViewData["InvoiceNumber"] = medicalPayment != null ? medicalPayment.InvoiceNo : string.Empty;
                //    ViewData["Amount"] = patientTest != null ? patientTest.TotalAmount.ToString() : string.Empty;
                //    ViewData["Discount"] = patientTest != null ? patientTest.LessAmount.ToString() : string.Empty;
                //    ViewData["DiscountAmount"] = round.ToString(); //patientTest != null ? patientTest.TotalAmount.ToString() : string.Empty;
                //    ViewData["Due"] = medicalPayment != null ? medicalPayment.DueAmount.ToString() : string.Empty;
                //    ViewData["Date"] = patientTest != null ? Utility.GetDateFormatByValue(patientTest.DateOfDelivery) : string.Empty;
                //    ViewData["Particulars"] = "Medical Test Payment";
                //}
            }

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_PatientTestInvoice", null);
            return Json(new { html = renderedView });
        }

        [HttpGet]
        public async Task<IActionResult> SavePaymentCollection(long patientTestId = 0, decimal totalAmount = 0, decimal paidAmount = 0, decimal dueAmount = 0, int discount = 0)
        {
            var isResultOk = false;
            var model = await _patientTestRepository.Get(patientTestId);
            decimal dueAmt = 0;
            decimal prePayAmt = 0;
            decimal discountAmt = 0;

            if (model != null)
            {
                if (discount != 0)
                {
                    discountAmt = Math.Round(((Convert.ToDecimal(discount) / 100) * totalAmount));
                    totalAmount = totalAmount - discountAmt;
                }

                if (model.DueAmount == null)
                {
                    dueAmt = totalAmount - paidAmount;
                }
                else
                {
                    prePayAmt = (model.TotalAmount ?? 0) - (model.DueAmount ?? 0);
                    dueAmt = (totalAmount - prePayAmt) - paidAmount;
                }

                //if(discountAmount != 0)
                //{
                //    decimal discountRate = discountAmount / 100;
                //    dueAmt = dueAmt - (discountRate * dueAmt);
                //}


                model.TotalAmount = totalAmount + discountAmt;
                model.DueAmount = dueAmt;// totalAmount - paidAmount;
                model.LessAmount = discount;
                //model.DiscountAmount = discountAmt;
                await _patientTestRepository.Save(model);
                isResultOk = true;

                await _medicalPaymentRepository.UpdateMedicalPayment(model.Id, EnumTransactionType.MedicalTest.ToString(), EnumPaymentType.Cash.ToString(), model.PatientId,
                    totalAmount, paidAmount, dueAmt, 0);

                await _accountHeadRepository.UpdateAccountHeadCurrentBalance(EnumTransactionType.MedicalTest.ToString(), paidAmount);
            }
            return Json(new { isResultOk = isResultOk, dueAmount = model.DueAmount });
        }

        [HttpGet]
        public async Task<IActionResult> GetMedicalTestRate(long id)
        {
            var isResultOk = false;
            var model = await _medicalTestRepository.Get(id);
            decimal testRate = 0;
            decimal totalAmount = 0;

            if (model != null)
            {
                testRate = model.Amount ?? 0;
                totalAmount = model.Amount ?? 0;
                isResultOk = true;
            }
            return Json(new { isResultOk = isResultOk, testRate = testRate, totalAmount = totalAmount });
        }

        [HttpGet]
        public async Task<IActionResult> GetMedicalTestRateWithDiscount(long id, decimal discount = 0)
        {
            var isResultOk = false;
            var model = await _medicalTestRepository.Get(id);
            decimal testRate = 0;
            decimal totalAmount = 0;
            decimal discountPercent = 0;
            decimal discountAmount = 0;

            if (model != null)
            {
                testRate = model.Amount ?? 0;
                discountPercent = discount / 100;
                discountAmount = testRate * discountPercent;
                totalAmount = (model.Amount ?? 0) - discountAmount;
                isResultOk = true;
            }
            return Json(new { isResultOk = isResultOk, testRate = testRate, totalAmount = totalAmount });
        }

        [HttpGet]
        public async Task<JsonResult> PrintInvoice(long patientTestId = 0)
        {
            if (patientTestId > 0)
            {
                //var patientTest = await _patientTestRepository.Get(patientTestId);

                //ViewData["DoctorName"] = patientTest.Doctor.Name;
                //ViewData["DoctorDegree"] = patientTest.Doctor.DegreeOther;
                //ViewData["DoctorDepartment"] = patientTest.Doctor.Department.Name;

                //ViewData["PatientName"] = patientTest.Patient.Name;
                //ViewData["PatientAge"] = patientTest.Patient.Age;
                //ViewData["PatientSex"] = patientTest.Patient.Gender;
                //ViewData["AppointmentDate"] = Utility.GetDateFormatByValue(patientTest.Appointment.DateOfAppointment);

                var patientTest = await _patientTestRepository.Get(patientTestId);
                var patientTestDetail = patientTest.PatientTestDetails.ToList();

                var patientTests = await _patientsRepository.GetAllActive();
                var patientTestDetails = await _patientTestDetailRepository.GetAllActive();

                var medicalTests = await _medicalTestRepository.GetAllActive();

                var patientTestInvoiceViewModel = from p in patientTests
                           join d in patientTestDetails on p.Id equals d.PatientTestId
                           join m in medicalTests on d.MedicalTestId equals m.Id
                           where p.Id == patientTestId
                           select new PatientTestInvoiceViewModel
                           {
                               Code = m.Code,
                               Name = m.Name,
                               Amount = d.Amount.ToString()
                           };

                ViewBag.PatientTestInvoice = patientTestInvoiceViewModel;

                var pTestDetailList = patientTestDetail.Select(b => new SelectListItem()
                {
                    Value = b.Id.ToString(),
                    Text = b.MedicalTest.Name.ToString()
                });



                ViewBag.ListMedicalPaymentDetail = new SelectList(pTestDetailList, "Value", "Text");

                var medicalPayment = await _medicalPaymentRepository.GetMedicalPaymentByTransactionId(patientTestId, EnumTransactionType.MedicalTest.ToString());
                var medicalPaymentDetail = medicalPayment.MedicalPaymentDetails.ToList();

                var enumList = medicalPaymentDetail.Select(b => new SelectListItem()
                {
                    Value = b.Id.ToString(),
                    Text = b.Amount.ToString()
                });
                ViewBag.ListMedicalPaymentDetail = new SelectList(enumList, "Value", "Text");

                if (patientTest != null && (patientTest.LessAmount == null || patientTest.LessAmount.Value == 0))
                {
                    ViewData["TotalCollection"] = medicalPaymentDetail.Select(a => a.Amount).Sum();
                    ViewData["InvoiceNumber"] = medicalPayment != null ? medicalPayment.InvoiceNo : string.Empty;
                    ViewData["Amount"] = patientTest != null ? patientTest.TotalAmount.ToString() : string.Empty;
                    ViewData["Discount"] = "0";
                    ViewData["DiscountAmount"] = patientTest != null ? patientTest.TotalAmount.ToString() : string.Empty;
                    ViewData["Due"] = medicalPayment != null ? medicalPayment.DueAmount.ToString() : string.Empty;
                    //ViewData["Date"] = patientTest != null ? Utility.GetDateFormatByValue(patientTest.DateOfDelivery) : string.Empty;
                    ViewData["Date"] = (patientTest != null && patientTest.DeliveryDate.HasValue) ? patientTest.DeliveryDate.Value.ToString(Constants.DateFormat) : string.Empty;
                    ViewData["Particulars"] = "Medical Test Payment";
                }
                else
                {
                    var discount = patientTest.LessAmount.Value;
                    decimal discountPercent = Convert.ToDecimal(discount) / 100;
                    var discountTotal = patientTest.TotalAmount - (discountPercent * patientTest.TotalAmount);

                    var round = Math.Round(discountTotal.Value);

                    ViewData["TotalCollection"] = round.ToString(); //medicalPaymentDetail.Select(a => a.Amount).Sum();
                    ViewData["InvoiceNumber"] = medicalPayment != null ? medicalPayment.InvoiceNo : string.Empty;
                    ViewData["Amount"] = patientTest != null ? patientTest.TotalAmount.ToString() : string.Empty;
                    ViewData["Discount"] = patientTest != null ? patientTest.LessAmount.ToString() : string.Empty;
                    ViewData["DiscountAmount"] = round.ToString(); //patientTest != null ? patientTest.TotalAmount.ToString() : string.Empty;
                    ViewData["Due"] = medicalPayment != null ? medicalPayment.DueAmount.ToString() : string.Empty;
                    //ViewData["Date"] = patientTest != null ? Utility.GetDateFormatByValue(patientTest.DateOfDelivery) : string.Empty;
                    ViewData["Date"] = (patientTest != null && patientTest.DeliveryDate.HasValue) ? patientTest.DeliveryDate.Value.ToString(Constants.DateFormat) : string.Empty;
                    ViewData["Particulars"] = "Medical Test Payment";
                }
            }

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_PatientTestInvoice", null);
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
        public async Task<IActionResult> Delete(long id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var model = await _patientTestRepository.Get(id);
                if (model != null)
                {
                    name = string.Empty; //model.Appointment.AppointmentNo.ToString().PadLeft(3, '0');
                }
            }
            return PartialView("DisplayTemplates/_DeletePatientTest", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectDeleteViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Id.ToString()) && model.Id != 0)
            {
                var pTest = await _patientTestRepository.Get(model.Id);
                if (pTest != null)
                {
                    pTest.IsDeleted = true;
                    await _patientTestRepository.Update(pTest);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
    }
}
