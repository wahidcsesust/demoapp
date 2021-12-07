using Microsoft.AspNetCore.Mvc;
using HealthCare.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using HealthCare.Web.Models.Report;
using HealthCare.Data.Services;
using System.Linq;
using HealthCare.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Globalization;
using HealthCare.Data.Common;
using HealthCare.Data.Interfaces;
using HealthCare.Web.Services;
using System.Data.SqlClient;
using Dapper;

namespace HealthCare.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMemberRepository _memberRepository;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IGenericRepository<Collection> _collectionRepository;
        private readonly IGenericRepository<MiscellaneousCollection> _miscellaneousRepository;
        private readonly IGenericRepository<MiscellaneousType> _miscellaneousTypeRepository;
        private readonly IGenericRepository<Loan> _loanRepository;
        private readonly IGenericRepository<LoanCollection> _loanCollectionRepository;
        private readonly IGenericRepository<Expense> _expenseRepository;
        private readonly IGenericRepository<PharmacyIncome> _pharmacyIncomeRepository;
        private readonly IGenericRepository<CashBackCollection> _cashBackRepository;
        private readonly IGenericRepository<SecurityAdvance> _securityAdvanceRepository;
        private readonly IGenericRepository<AccountHead> _accountHeadRepository;
        private readonly IGenericRepository<Doctor> _doctorRepository; 
        private readonly IGenericRepository<Patient> _patientsRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalPaymentRepository _medicalPaymentRepository;
        public ReportController(
            UserManager<User> userManager,
            IMemberRepository memberRepository,
            IHostingEnvironment hostEnvironment,
            ICompositeViewEngine viewEngine,
            IGenericRepository<Collection> collectionRepository,
            IGenericRepository<MiscellaneousCollection> miscellaneousRepository,
            IGenericRepository<MiscellaneousType> miscellaneousTypeRepository,
            IGenericRepository<Loan> loanRepository,
            IGenericRepository<LoanCollection> loanCollectionRepository,
            IGenericRepository<Expense> expenseRepository,
            IGenericRepository<PharmacyIncome> pharmacyIncomeRepository,
            IGenericRepository<CashBackCollection> cashBackRepository,
            IGenericRepository<SecurityAdvance> securityAdvanceRepository,
            IGenericRepository<AccountHead> accountHeadRepository,
            IGenericRepository<Doctor> doctorRepository,
            IGenericRepository<Patient> patientsRepository,
            IAppointmentRepository appointmentRepository,
            IMedicalPaymentRepository medicalPaymentRepository
            )
        {
            _userManager = userManager;
            _memberRepository = memberRepository;
            _hostEnvironment = hostEnvironment;
            _viewEngine = viewEngine;
            _collectionRepository = collectionRepository;
            _miscellaneousRepository = miscellaneousRepository;
            _miscellaneousTypeRepository = miscellaneousTypeRepository;
            _loanRepository = loanRepository;
            _loanCollectionRepository = loanCollectionRepository;
            _expenseRepository = expenseRepository;
            _pharmacyIncomeRepository = pharmacyIncomeRepository;
            _cashBackRepository = cashBackRepository;
            _securityAdvanceRepository = securityAdvanceRepository;
            _accountHeadRepository = accountHeadRepository;
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _patientsRepository = patientsRepository;
            _medicalPaymentRepository = medicalPaymentRepository;
        }

        public IActionResult Manage()
        {
            return View();
        }

        [HttpGet]
        public async Task<PartialViewResult> OpenReportModal(int type = 0)
        {
            var model = new ReportViewModel();
            model.ObjectName = "Member";
            if (type == 1)
            {
                var doctors = await _doctorRepository.GetAllActive();

                model.Objects = doctors.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();
            }
            //else if (type == (int)ReportType.ExpenseReport)
            //{
            //    var models = await _expenseRepository.GetAllActive();

            //    model.Objects = models.Select(x => new SelectListItem
            //    {
            //        Value = x.Id.ToString(),
            //        Text = x.InvoiceNo
            //    }).ToList();
            //}

            switch (type)
            {

                case (int)ReportTypeEnum.MemberReport:
                    model.ReportTypeEnum = ReportTypeEnum.MemberReport;
                    model.ReportHeading = "Member Report";
                    break;
                //case (int)ReportTypeEnum.DoctorVisit:
                //    model.ReportType = ReportTypeEnum.DoctorVisit;
                //    model.ReportHeading = "Doctor Visit Report";
                //    model.DateFrom = string.Format("{0}/{1}/{2}", DateTime.Today.AddMonths(-1).Month.ToString().PadLeft(2, '0'), DateTime.Today.Day.ToString().PadLeft(2, '0'), DateTime.Today.Year.ToString());
                //    model.DateTo = string.Format("{0}/{1}/{2}", DateTime.Today.Month.ToString().PadLeft(2, '0'), DateTime.Today.Day.ToString().PadLeft(2, '0'), DateTime.Today.Year.ToString());
                //    model.DateFrom = "01-07-2020";
                //    model.DateTo = DateTime.Now.ToString(Constants.DateFormat);
                //    break;
                //case (int)ReportType.MedicalTest:
                //    model.ReportType = ReportType.MedicalTest;
                //    model.ReportHeading = "Medical Test Report";
                //    model.DateFrom = "01-07-2020";
                //    model.DateTo = DateTime.Now.ToString(Constants.DateFormat);
                //    break;
                //case (int)ReportType.Admission:
                //    model.ReportType = ReportType.Admission;
                //    model.ReportHeading = "Admission Report";
                //    model.DateFrom = "01-07-2020";
                //    model.DateTo = DateTime.Now.ToString(Constants.DateFormat);
                //    break;
                //case (int)ReportType.ExpenseReport:
                //    model.ReportType = ReportType.ExpenseReport;
                //    model.ReportHeading = "Expense Report";
                //    model.DateFrom = "01-07-2020";
                //    model.DateTo = DateTime.Now.ToString(Constants.DateFormat);
                //    break;
                //case (int)ReportType.PharmacyReport:
                //    model.ReportType = ReportType.PharmacyReport;
                //    model.ReportHeading = "Pharmacy Report";
                //    model.DateFrom = "01-07-2020";
                //    model.DateTo = DateTime.Now.ToString(Constants.DateFormat);
                //    break;
            }

            return PartialView("DisplayTemplates/_Report", model);
        }

        [HttpGet]
        public async Task<JsonResult> PrintMemberReport(int objectId, int areaLocationId, int bloodGroupId)
        {
            var strCategory = "All";
            var desCategory = "All";
            if (objectId == (int)MemberCategoryEnum.AdvisoryCouncil)
            {
                strCategory = MemberCategoryEnum.AdvisoryCouncil.ToString();
                desCategory = EnumHelper<MemberCategoryEnum>.GetDisplayValue(MemberCategoryEnum.AdvisoryCouncil);
            }
            else if (objectId == (int)MemberCategoryEnum.ExecutiveCouncil)
            {
                strCategory = MemberCategoryEnum.ExecutiveCouncil.ToString();
                desCategory = EnumHelper<MemberCategoryEnum>.GetDisplayValue(MemberCategoryEnum.ExecutiveCouncil);
            }
            else if (objectId == (int)MemberCategoryEnum.GeneralMember)
            {
                strCategory = MemberCategoryEnum.GeneralMember.ToString();
                desCategory = EnumHelper<MemberCategoryEnum>.GetDisplayValue(MemberCategoryEnum.GeneralMember);
            }
            else if (objectId == (int)MemberCategoryEnum.Other)
            {
                strCategory = MemberCategoryEnum.Other.ToString();
                desCategory = EnumHelper<MemberCategoryEnum>.GetDisplayValue(MemberCategoryEnum.Other);
            }

            var strBloodGroup = "All";
            var desBloodGroup = "All";
            if (bloodGroupId == (int)BloodGroupEnum.APositive)
            {
                strBloodGroup = BloodGroupEnum.APositive.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.APositive);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.BPostive)
            {
                strBloodGroup = BloodGroupEnum.BPostive.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.BPostive);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.ABPositive)
            {
                strBloodGroup = BloodGroupEnum.ABPositive.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.ABPositive);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.OPositive)
            {
                strBloodGroup = BloodGroupEnum.OPositive.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.OPositive);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.ANegative)
            {
                strBloodGroup = BloodGroupEnum.ANegative.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.ANegative);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.BNegative)
            {
                strBloodGroup = BloodGroupEnum.BNegative.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.BNegative);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.ABNegative)
            {
                strBloodGroup = BloodGroupEnum.ABNegative.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.ABNegative);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.ONegative)
            {
                strBloodGroup = BloodGroupEnum.ONegative.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.ONegative);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.NONE)
            {
                strBloodGroup = BloodGroupEnum.NONE.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.NONE);
            }



            var strAreaLocation = "All";
            var desAreaLocation = "All";
            if (areaLocationId == (int)AreaLocationEnum.East)
            {
                strAreaLocation = AreaLocationEnum.East.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.East);
            }
            else if (areaLocationId == (int)AreaLocationEnum.Middle)
            {
                strAreaLocation = AreaLocationEnum.Middle.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.Middle);
            }
            else if (areaLocationId == (int)AreaLocationEnum.North)
            {
                strAreaLocation = AreaLocationEnum.North.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.North);
            }
            else if (areaLocationId == (int)AreaLocationEnum.South)
            {
                strAreaLocation = AreaLocationEnum.South.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.South);
            }
            else if (areaLocationId == (int)AreaLocationEnum.West)
            {
                strAreaLocation = AreaLocationEnum.West.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.West);
            }
            else if (areaLocationId == (int)AreaLocationEnum.Other)
            {
                strBloodGroup = AreaLocationEnum.Other.ToString();
                desBloodGroup = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.Other);
            }

            var members = await _memberRepository.GetMemberListByCategoryId(strCategory, strAreaLocation, strBloodGroup);

            var memberList = members.Select(a => new ReportViewModel
            {
                RegNo = a.RegNo,
                ObjectName = a.Name,
                FatherName = a.FatherName,
                BloodGroup = a.BloodGroupString,
                Profession = a.Profession,
                Category = a.CategoryString,
                Designation = a.DesignationString,
                ImageName = a.ImageName,
                Age = a.Age,
                MobileNumber = a.MobileNumber
            });

            ViewData["Category"] = desCategory;
            ViewData["AreaLocation"] = desAreaLocation;
            ViewData["BloodGroup"] = desBloodGroup;
            ViewData["Total"] = memberList.Count();

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_MemberReport", memberList.OrderBy(m => m.RegNo));
            return Json(new { html = renderedView });
        }
        [HttpGet]
        public async Task<JsonResult> PrintMemberReportCollection(int objectId, int areaLocationId, int bloodGroupId)
        {
            var strCategory = "All";
            var desCategory = "All";
            if (objectId == (int)MemberCategoryEnum.AdvisoryCouncil)
            {
                strCategory = MemberCategoryEnum.AdvisoryCouncil.ToString();
                desCategory = EnumHelper<MemberCategoryEnum>.GetDisplayValue(MemberCategoryEnum.AdvisoryCouncil);
            }
            else if (objectId == (int)MemberCategoryEnum.ExecutiveCouncil)
            {
                strCategory = MemberCategoryEnum.ExecutiveCouncil.ToString();
                desCategory = EnumHelper<MemberCategoryEnum>.GetDisplayValue(MemberCategoryEnum.ExecutiveCouncil);
            }
            else if (objectId == (int)MemberCategoryEnum.GeneralMember)
            {
                strCategory = MemberCategoryEnum.GeneralMember.ToString();
                desCategory = EnumHelper<MemberCategoryEnum>.GetDisplayValue(MemberCategoryEnum.GeneralMember);
            }
            else if (objectId == (int)MemberCategoryEnum.Other)
            {
                strCategory = MemberCategoryEnum.Other.ToString();
                desCategory = EnumHelper<MemberCategoryEnum>.GetDisplayValue(MemberCategoryEnum.Other);
            }

            var strBloodGroup = "All";
            var desBloodGroup = "All";
            if (bloodGroupId == (int)BloodGroupEnum.APositive)
            {
                strBloodGroup = BloodGroupEnum.APositive.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.APositive);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.BPostive)
            {
                strBloodGroup = BloodGroupEnum.BPostive.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.BPostive);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.ABPositive)
            {
                strBloodGroup = BloodGroupEnum.ABPositive.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.ABPositive);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.OPositive)
            {
                strBloodGroup = BloodGroupEnum.OPositive.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.OPositive);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.ANegative)
            {
                strBloodGroup = BloodGroupEnum.ANegative.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.ANegative);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.BNegative)
            {
                strBloodGroup = BloodGroupEnum.BNegative.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.BNegative);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.ABNegative)
            {
                strBloodGroup = BloodGroupEnum.ABNegative.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.ABNegative);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.ONegative)
            {
                strBloodGroup = BloodGroupEnum.ONegative.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.ONegative);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.NONE)
            {
                strBloodGroup = BloodGroupEnum.NONE.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.NONE);
            }



            var strAreaLocation = "All";
            var desAreaLocation = "All";
            if (areaLocationId == (int)AreaLocationEnum.East)
            {
                strAreaLocation = AreaLocationEnum.East.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.East);
            }
            else if (areaLocationId == (int)AreaLocationEnum.Middle)
            {
                strAreaLocation = AreaLocationEnum.Middle.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.Middle);
            }
            else if (areaLocationId == (int)AreaLocationEnum.North)
            {
                strAreaLocation = AreaLocationEnum.North.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.North);
            }
            else if (areaLocationId == (int)AreaLocationEnum.South)
            {
                strAreaLocation = AreaLocationEnum.South.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.South);
            }
            else if (areaLocationId == (int)AreaLocationEnum.West)
            {
                strAreaLocation = AreaLocationEnum.West.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.West);
            }
            else if (areaLocationId == (int)AreaLocationEnum.Other)
            {
                strBloodGroup = AreaLocationEnum.Other.ToString();
                desBloodGroup = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.Other);
            }

            var members = await _memberRepository.GetMemberListByCategoryId(strCategory, strAreaLocation, strBloodGroup);

            var memberList = members.Select(a => new ReportViewModel
            {
                RegNo = a.RegNo,
                ObjectName = a.Name,
                FatherName = a.FatherName,
                BloodGroup = a.BloodGroupString,
                Profession = a.Profession,
                Category = a.CategoryString,
                Designation = a.DesignationString,
                ImageName = a.ImageName,
                Age = a.Age,
                MobileNumber = a.MobileNumber
            });

            ViewData["Category"] = desCategory;
            ViewData["AreaLocation"] = desAreaLocation;
            ViewData["BloodGroup"] = desBloodGroup;
            ViewData["Total"] = memberList.Count();

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_MemberReportCollection", memberList.OrderBy(m => m.RegNo));
            return Json(new { html = renderedView });
        }
        [HttpGet]
        public async Task<JsonResult> PrintMemberReportDailyCollection(int objectId, int areaLocationId, int bloodGroupId)
        {
            var strCategory = "All";
            var desCategory = "All";
            if (objectId == (int)MemberCategoryEnum.AdvisoryCouncil)
            {
                strCategory = MemberCategoryEnum.AdvisoryCouncil.ToString();
                desCategory = EnumHelper<MemberCategoryEnum>.GetDisplayValue(MemberCategoryEnum.AdvisoryCouncil);
            }
            else if (objectId == (int)MemberCategoryEnum.ExecutiveCouncil)
            {
                strCategory = MemberCategoryEnum.ExecutiveCouncil.ToString();
                desCategory = EnumHelper<MemberCategoryEnum>.GetDisplayValue(MemberCategoryEnum.ExecutiveCouncil);
            }
            else if (objectId == (int)MemberCategoryEnum.GeneralMember)
            {
                strCategory = MemberCategoryEnum.GeneralMember.ToString();
                desCategory = EnumHelper<MemberCategoryEnum>.GetDisplayValue(MemberCategoryEnum.GeneralMember);
            }
            else if (objectId == (int)MemberCategoryEnum.Other)
            {
                strCategory = MemberCategoryEnum.Other.ToString();
                desCategory = EnumHelper<MemberCategoryEnum>.GetDisplayValue(MemberCategoryEnum.Other);
            }

            var strBloodGroup = "All";
            var desBloodGroup = "All";
            if (bloodGroupId == (int)BloodGroupEnum.APositive)
            {
                strBloodGroup = BloodGroupEnum.APositive.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.APositive);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.BPostive)
            {
                strBloodGroup = BloodGroupEnum.BPostive.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.BPostive);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.ABPositive)
            {
                strBloodGroup = BloodGroupEnum.ABPositive.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.ABPositive);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.OPositive)
            {
                strBloodGroup = BloodGroupEnum.OPositive.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.OPositive);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.ANegative)
            {
                strBloodGroup = BloodGroupEnum.ANegative.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.ANegative);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.BNegative)
            {
                strBloodGroup = BloodGroupEnum.BNegative.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.BNegative);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.ABNegative)
            {
                strBloodGroup = BloodGroupEnum.ABNegative.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.ABNegative);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.ONegative)
            {
                strBloodGroup = BloodGroupEnum.ONegative.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.ONegative);
            }
            else if (bloodGroupId == (int)BloodGroupEnum.NONE)
            {
                strBloodGroup = BloodGroupEnum.NONE.ToString();
                desBloodGroup = EnumHelper<BloodGroupEnum>.GetDisplayValue(BloodGroupEnum.NONE);
            }



            var strAreaLocation = "All";
            var desAreaLocation = "All";
            if (areaLocationId == (int)AreaLocationEnum.East)
            {
                strAreaLocation = AreaLocationEnum.East.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.East);
            }
            else if (areaLocationId == (int)AreaLocationEnum.Middle)
            {
                strAreaLocation = AreaLocationEnum.Middle.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.Middle);
            }
            else if (areaLocationId == (int)AreaLocationEnum.North)
            {
                strAreaLocation = AreaLocationEnum.North.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.North);
            }
            else if (areaLocationId == (int)AreaLocationEnum.South)
            {
                strAreaLocation = AreaLocationEnum.South.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.South);
            }
            else if (areaLocationId == (int)AreaLocationEnum.West)
            {
                strAreaLocation = AreaLocationEnum.West.ToString();
                desAreaLocation = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.West);
            }
            else if (areaLocationId == (int)AreaLocationEnum.Other)
            {
                strBloodGroup = AreaLocationEnum.Other.ToString();
                desBloodGroup = EnumHelper<AreaLocationEnum>.GetDisplayValue(AreaLocationEnum.Other);
            }

            var members = await _memberRepository.GetMemberListByCategoryId(strCategory, strAreaLocation, strBloodGroup);

            var memberList = members.Select(a => new ReportViewModel
            {
                RegNo = a.RegNo,
                ObjectName = a.Name,
                FatherName = a.FatherName,
                BloodGroup = a.BloodGroupString,
                Profession = a.Profession,
                Category = a.CategoryString,
                Designation = a.DesignationString,
                ImageName = a.ImageName,
                Age = a.Age,
                MobileNumber = a.MobileNumber
            });

            ViewData["Category"] = desCategory;
            ViewData["AreaLocation"] = desAreaLocation;
            ViewData["BloodGroup"] = desBloodGroup;
            ViewData["Total"] = memberList.Count();

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_MemberReportDailyCollection", memberList.OrderBy(m => m.RegNo));
            return Json(new { html = renderedView });
        }

        [HttpGet]
        public async Task<JsonResult> PrintDoctorAppointment(long doctorId, string dateOfAppointment)
        {
            var appointments = await _appointmentRepository.GetAppointmentByDoctorId(doctorId, dateOfAppointment.StringToDate());
            var doctor = await _doctorRepository.Get(doctorId);

            var patientList = appointments.Select(a => new ReportViewModel
            {
                SerialNo = a.SerialNo,
                PatientName = a.Patient.Name
            });

            ViewData["Name"] = doctor != null ? doctor.Name : string.Empty;
            ViewData["DateOfAppointment"] = dateOfAppointment;

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_DoctorAppointment", patientList.OrderBy(m => m.SerialNo));
            return Json(new { html = renderedView });
        }

        [HttpGet]
        public async Task<JsonResult> PrintDailyCollection(string dateOfCollection = null)
        {
            var models = _medicalPaymentRepository.GetAllActive().Result;

            var totalDoctorVisit = models.Where(c => c.TransactionType == "DoctorVisit").Select(a => a.TotalAmount).Sum();
            var totalMedicalTest = models.Where(c => c.TransactionType == "MedicalTest").Select(a => a.TotalAmount).Sum();

            ViewData["DoctorVisit"] = totalDoctorVisit.ToString();
            ViewData["MedicalTest"] = totalMedicalTest.ToString();

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_DailyCollection", null);
            return Json(new { html = renderedView });
        }

        [HttpGet]
        public async Task<JsonResult> PrintDoctorVisit(string fromDate = null, string toDate = null)
        {
            var query = $@"SELECT pt.Name PatientName, mp.InvoiceDate, isnull(mp.TotalAmount,0) Amount, ISNULL(mpd.Amount,0) PaidAmount, isnull(mp.DueAmount,0) DueAmount, isnull(mp.LessAmount,0) LessAmount FROM MedicalPayments mp
                            INNER JOIN
                            (
                            select MedicalPaymentId, isnull(sum(isnull(Amount,0)),0) Amount from MedicalPaymentDetails GROUP BY MedicalPaymentId
                            ) mpd on mpd.MedicalPaymentId=mp.Id
                            LEFT JOIN Patients pt on pt.Id=mp.PatientId
                            WHERE mp.TransactionType='DoctorVisit' AND mp.InvoiceDate BETWEEN '{fromDate.StringToDate()}' AND '{toDate.StringToLastMinuteOfDate()}'
                            ORDER BY mp.PatientId, mp.InvoiceDate";

            var reportViewModel = new List<ReportViewModel>();
            using (var con = new SqlConnection(Constants.ConnectionString))
            {
                reportViewModel = con.Query<ReportViewModel>(query).ToList();
            }



            ViewData["TotalAmount"] = reportViewModel.Select(a => a.Amount).Sum();
            ViewData["TotalPaid"] = reportViewModel.Select(a => a.PaidAmount).Sum();
            ViewData["TotalDue"] = reportViewModel.Select(a => a.DueAmount).Sum();
            //ViewData["TotalLess"] = reportViewModel.Select(a => a.LessAmount).Sum();

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_DoctorVisit", reportViewModel);
            return Json(new { html = renderedView });
        }

        [HttpGet]
        public async Task<JsonResult> PrintExpenseNPharmacyReport(string reportType, string fromDate = null, string toDate = null)
        {
            var query = $@"select Id ObjectId, InvoiceNo Invoice, Particulars Particular,* FROM Expenses WHERE IsDeleted = 0 AND CollectionDate BETWEEN '{fromDate.StringToDate()}' AND '{toDate.StringToLastMinuteOfDate()}'
                            ORDER BY Id, CollectionDate";
            if(reportType == ReportType.PharmacyReport.ToString())
                query = $@"select Id ObjectId, InvoiceNo Invoice, Particulars Particular,* FROM PharmacyIncomes WHERE IsDeleted = 0 AND CollectionDate BETWEEN '{fromDate.StringToDate()}' AND '{toDate.StringToLastMinuteOfDate()}'
                            ORDER BY Id, CollectionDate";

            var reportViewModel = new List<ReportViewModel>();
            using (var con = new SqlConnection(Constants.ConnectionString))
            {
                reportViewModel = con.Query<ReportViewModel>(query).ToList();
            }

            ViewData["TotalAmount"] = reportViewModel.Select(a => a.Amount).Sum();
            ViewData["ReportType"] = reportType;

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_ExpenseReport", reportViewModel);
            return Json(new { html = renderedView });
        }

        [HttpGet]
        public async Task<JsonResult> PrintMedicalTest(string fromDate = null, string toDate = null)
        {
            var query = $@"SELECT pt.Name PatientName, mp.InvoiceDate, isnull(mp.TotalAmount,0) Amount, ISNULL(mpd.Amount,0) PaidAmount, isnull(mp.DueAmount,0) DueAmount, isnull(mp.LessAmount,0) LessAmount FROM MedicalPayments mp
                            INNER JOIN
                            (
                            select MedicalPaymentId, isnull(sum(isnull(Amount,0)),0) Amount from MedicalPaymentDetails GROUP BY MedicalPaymentId
                            ) mpd on mpd.MedicalPaymentId=mp.Id
                            LEFT JOIN Patients pt on pt.Id=mp.PatientId
                            WHERE mp.TransactionType='MedicalTest' AND mp.InvoiceDate BETWEEN '{fromDate.StringToDate()}' AND '{toDate.StringToLastMinuteOfDate()}'
                            ORDER BY mp.PatientId, mp.InvoiceDate";

            var reportViewModel = new List<ReportViewModel>();
            using (var con = new SqlConnection(Constants.ConnectionString))
            {
                reportViewModel = con.Query<ReportViewModel>(query).ToList();
            }



            ViewData["TotalAmount"] = reportViewModel.Select(a => a.Amount).Sum();
            ViewData["TotalPaid"] = reportViewModel.Select(a => a.PaidAmount).Sum();
            ViewData["TotalDue"] = reportViewModel.Select(a => a.DueAmount).Sum();
            ViewData["TotalLess"] = reportViewModel.Select(a => a.LessAmount).Sum();

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_MedicalTest", reportViewModel);
            return Json(new { html = renderedView });
        }

        [HttpGet]
        public async Task<JsonResult> PrintAdmissionReport(string fromDate = null, string toDate = null)
        {
            //var medicalPayments = await _medicalPaymentRepository.GetAllActive();

            //var medicalPaymentList = medicalPayments.Select(a => new ReportViewModel
            //{
            //    PatientId = a.PatientId,
            //    PatientName = a.Patient.Name,
            //    Amount = a.TotalAmount ?? 0,
            //    LessAmount = a.LessAmount ?? 0,
            //    DueAmount = a.DueAmount ?? 0,
            //    TransactionType = a.TransactionType,
            //    CollectionDate = Utility.GetDateByValue(a.DateOfInvoice),
            //    DateOfCollection = Utility.GetDateFormatByValue(a.DateOfInvoice)
            //});

            //var filterList = medicalPaymentList.Where(f => f.TransactionType == "Admission" && f.CollectionDate >= fromDate.StringToDate() && f.CollectionDate <= toDate.StringToDate());
            //var finalList = filterList.OrderBy(x => x.PatientId).ThenBy(x => x.CollectionDate.TimeOfDay).ThenBy(x => x.CollectionDate.Date).ThenBy(x => x.CollectionDate.Year);

            //ViewData["TotalAmount"] = finalList.Select(a => a.Amount).Sum();
            //ViewData["TotalDue"] = finalList.Select(a => a.DueAmount).Sum();
            //ViewData["TotalLess"] = finalList.Select(a => a.LessAmount).Sum();

            //var renderedView = await RenderPartialViewToString("DisplayTemplates/_AdmissionReport", finalList);
            //return Json(new { html = renderedView });

            var query = $@"SELECT pt.Name PatientName, mp.InvoiceDate, isnull(mp.TotalAmount,0) Amount, ISNULL(mpd.Amount,0) PaidAmount, isnull(mp.DueAmount,0) DueAmount, isnull(mp.LessAmount,0) LessAmount FROM MedicalPayments mp
                            INNER JOIN
                            (
                            select MedicalPaymentId, isnull(sum(isnull(Amount,0)),0) Amount from MedicalPaymentDetails GROUP BY MedicalPaymentId
                            ) mpd on mpd.MedicalPaymentId=mp.Id
                            LEFT JOIN Patients pt on pt.Id=mp.PatientId
                            WHERE mp.TransactionType='Admission' AND mp.InvoiceDate BETWEEN '{fromDate.StringToDate()}' AND '{toDate.StringToLastMinuteOfDate()}'
                            ORDER BY mp.PatientId, mp.InvoiceDate";

            var reportViewModel = new List<ReportViewModel>();
            using (var con = new SqlConnection(Constants.ConnectionString))
            {
                reportViewModel = con.Query<ReportViewModel>(query).ToList();
            }



            ViewData["TotalAmount"] = reportViewModel.Select(a => a.Amount).Sum();
            ViewData["TotalPaid"] = reportViewModel.Select(a => a.PaidAmount).Sum();
            ViewData["TotalDue"] = reportViewModel.Select(a => a.DueAmount).Sum();
            ViewData["TotalLess"] = reportViewModel.Select(a => a.LessAmount).Sum();

            var renderedView = await RenderPartialViewToString("DisplayTemplates/_MedicalTest", reportViewModel);
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
    }
}