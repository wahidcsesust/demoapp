using HealthCare.Data.Interfaces;
using HealthCare.Data.Models;
using HealthCare.Data.Services;
using HealthCare.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Web.Controllers
{
    public class SmsController : Controller
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ISmsService _smsService;
        public SmsController(IMemberRepository memberRepository, ISmsService smsService)
        {
            _memberRepository = memberRepository;
            _smsService = smsService;
        }
        public IActionResult Manage()
        {
            SmsViewModel model = new SmsViewModel()
            {
            };
            var members = _memberRepository.GetAllActive().Result.OrderBy(r => r.RegNo).ToList();
            model.Members = members.Select(x => new SelectListItem
            {
                Text = x.Name + " (" + x.RegNo.ToString().PadLeft(4, '0') + ")",
                Value = x.Id.ToString()
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SendSms(long categoryId, string message)
        {
            var isResultOk = false;

            var strCategory = "All";
            if (categoryId == (int)MemberCategoryEnum.AdvisoryCouncil)
            {
                strCategory = MemberCategoryEnum.AdvisoryCouncil.ToString();
            }
            else if (categoryId == (int)MemberCategoryEnum.ExecutiveCouncil)
            {
                strCategory = MemberCategoryEnum.ExecutiveCouncil.ToString();
            }
            else if (categoryId == (int)MemberCategoryEnum.GeneralMember)
            {
                strCategory = MemberCategoryEnum.GeneralMember.ToString();
            }
            else if (categoryId == (int)MemberCategoryEnum.Other)
            {
                strCategory = MemberCategoryEnum.Other.ToString();
            }

            var members = await _memberRepository.GetMemberListByCategoryId(strCategory, "All", "All");
            var memberMobiles = members.Where(m => m.MobileNumber != null).Select(a => a.MobileNumber).ToList();
            string joinedMobiles = string.Join(",", memberMobiles);

            if (_smsService.SendSms(joinedMobiles, message))
            {
                isResultOk = true;
            }
            return Json(new { isResultOk = isResultOk });
        }
        [HttpGet]
        public async Task<IActionResult> SendSingleSms(long memberId, string message)
        {
            var isResultOk = false;

            var member = await _memberRepository.Get(memberId);
            var mobileNumber = member != null? member.MobileNumber: "01719012103";

            if (_smsService.SendSms(mobileNumber, message))
            {
                isResultOk = true;
            }
            return Json(new { isResultOk = isResultOk });
        }

        [HttpPost]
        public async Task<IActionResult> Send(SmsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var strCategory = model.MemberCategoryEnum.ToString();

                var members = await _memberRepository.GetMemberListByCategoryId(strCategory, "All", "All");

                var memberMobiles = members.Select(a => a.MobileNumber).ToList();

                string joinedMobiles = string.Join(",", memberMobiles);


                //var result = _smsService.SendSms(joinedMobiles, model.Message);

                return RedirectToAction("Manage");
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Manage");
        }
    }
}
