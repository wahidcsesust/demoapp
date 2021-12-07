using HealthCare.Data.Models;
using HealthCare.Data.Services;
using System;
using System.Globalization;

namespace HealthCare.Data.Common
{
    public static class Utility
    {
        public static DateTime GetDateByValue(string strDate)
        {
            return new DateTime(Convert.ToInt32(strDate.Split('/')[2]),
                Convert.ToInt32(strDate.Split('/')[0]), Convert.ToInt32(strDate.Split('/')[1]));
        }

        public static string GetCurrentDateString()
        {
            return string.Format("{0}/{1}/{2}", DateTime.Today.Month.ToString().PadLeft(2, '0'), DateTime.Today.Day.ToString().PadLeft(2, '0'), DateTime.Today.Year.ToString());
        }

        public static string GetDateFormatByValue(string strDate)
        {
            return string.IsNullOrEmpty(strDate) ? "" : string.Format("{0}-{1}-{2}", strDate.Split('/')[1],
                CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(strDate.Split('/')[0])).Substring(0, 3),
                strDate.Split('/')[2]);
        }

        private static string ShortMonthNameByDate(DateTime date)
        {
            return date.ToString("MMM", CultureInfo.InvariantCulture);
        }
        public static string RegNoPadding(int regNo)
        {
            return regNo.ToString().PadLeft(6, '0');
        }
        public static string RegNoNameCombine(int regNo, string name)
        {
            return string.Format("{0}-{1}", regNo.ToString(), name);
        }
        public static string GetAppointNo(int sequenceNo, int regNo)
        {
            return string.Format("{0}-{1}-{2}{3}{4}", sequenceNo.ToString().PadLeft(4,'0'), regNo.ToString().PadLeft(4, '0'), DateTime.Today.Day.ToString().PadLeft(2, '0'), DateTime.Today.Month.ToString().PadLeft(2, '0'), DateTime.Today.Year.ToString());
        }
        public static decimal GetTotalInterestByIntRateAndAmount(decimal interestRate, decimal amount)
        {
            decimal totalInterest = 0;
            totalInterest = interestRate / 100 * amount;
            return totalInterest;
        }
        public static decimal GetInstallmentAmountByDuration(decimal duration, decimal amount)
        {
            decimal installmentAmount = 0;
            installmentAmount = amount / duration;
            return installmentAmount;
        }

        public static BloodGroupEnum GetBloodGroup(string bloodGroup)
        {
            switch (bloodGroup)
            {
                case "APositive":
                    return BloodGroupEnum.APositive;
                case "BPostive":
                    return BloodGroupEnum.BPostive;
                case "ABPositive":
                    return BloodGroupEnum.ABPositive;
                case "OPositive":
                    return BloodGroupEnum.OPositive;
                case "ANegative":
                    return BloodGroupEnum.ANegative;
                case "BNegative":
                    return BloodGroupEnum.BNegative;
                case "ABNegative":
                    return BloodGroupEnum.ABNegative;
                case "ONegative":
                    return BloodGroupEnum.ONegative;
                case "NONE":
                    return BloodGroupEnum.NONE;
                default:
                    return BloodGroupEnum.NONE;
            }
        }
        public static AreaLocationEnum GetAreaLocation(string area)
        {
            switch (area)
            {
                case "East":
                    return AreaLocationEnum.East;
                case "West":
                    return AreaLocationEnum.West;
                case "North":
                    return AreaLocationEnum.North;
                case "South":
                    return AreaLocationEnum.South;
                case "Middle":
                    return AreaLocationEnum.Middle;
                case "Other":
                    return AreaLocationEnum.Other;
                default:
                    return AreaLocationEnum.East;
            }
        }


        public static MemberCategoryEnum GetMemberCategory(string value)
        {
            switch (value)
            {
                case "AdvisoryCouncil":
                    return MemberCategoryEnum.AdvisoryCouncil;
                case "ExecutiveCouncil":
                    return MemberCategoryEnum.ExecutiveCouncil;
                case "GeneralMember":
                    return MemberCategoryEnum.GeneralMember;
                case "Other":
                    return MemberCategoryEnum.Other;
                default:
                    return MemberCategoryEnum.ExecutiveCouncil;
            }
        }
        public static MemberDesignationEnum GetMemberDesignation(string value)
        {
            switch (value)
            {
                case "Advisor":
                    return MemberDesignationEnum.Advisor;
                case "President":
                    return MemberDesignationEnum.President;
                case "VicePresident":
                    return MemberDesignationEnum.VicePresident;

                case "GeneralSecretary":
                    return MemberDesignationEnum.GeneralSecretary;
                case "JointSecretary":
                    return MemberDesignationEnum.JointSecretary;
                case "OrganizationalSecretary":
                    return MemberDesignationEnum.OrganizationalSecretary;

                case "ViceOrganizationalSecretary":
                    return MemberDesignationEnum.ViceOrganizationalSecretary;
                case "PublicitySecretary":
                    return MemberDesignationEnum.PublicitySecretary;
                case "VicePublicitySecretary":
                    return MemberDesignationEnum.VicePublicitySecretary;

                case "FinanceSecretary":
                    return MemberDesignationEnum.FinanceSecretary;
                case "SocialWelfareSecretary":
                    return MemberDesignationEnum.SocialWelfareSecretary;
                case "SportsAndCultureSecretary":
                    return MemberDesignationEnum.SportsAndCultureSecretary;

                case "OfficeSecretary":
                    return MemberDesignationEnum.OfficeSecretary;
                case "ViceOfficeSecretary":
                    return MemberDesignationEnum.ViceOfficeSecretary;
                case "ReligiousAffairsSecretary":
                    return MemberDesignationEnum.ReligiousAffairsSecretary;
                case "LegalSecretary":
                    return MemberDesignationEnum.LegalSecretary;

                case "WomenAffairsSecretary":
                    return MemberDesignationEnum.WomenAffairsSecretary;
                case "EnvironmentalSecretary":
                    return MemberDesignationEnum.EnvironmentalSecretary;
                case "LibraryAffairsSecretary":
                    return MemberDesignationEnum.LibraryAffairsSecretary;
                case "HealthAffairsSecretary":
                    return MemberDesignationEnum.HealthAffairsSecretary;
                case "GeneralMember":
                    return MemberDesignationEnum.Member;
                case "Other":
                    return MemberDesignationEnum.Other;
                default:
                    return MemberDesignationEnum.Member;
            }
        }

        public static ProfessionEnum GetProfession(string value)
        {
            switch (value)
            {
                case "Job":
                    return ProfessionEnum.Job;
                case "Business":
                    return ProfessionEnum.Business;
                case "Student":
                    return ProfessionEnum.Student;
                case "Other":
                    return ProfessionEnum.Other;
                default:
                    return ProfessionEnum.Job;
            }
        }
        public static ReligionEnum GetReligion(string value)
        {
            switch (value)
            {
                case "Islam":
                    return ReligionEnum.Islam;
                case "Hinduism":
                    return ReligionEnum.Hinduism;
                case "Buddhism":
                    return ReligionEnum.Buddhism;
                case "Christianity":
                    return ReligionEnum.Christianity;
                case "Other":
                    return ReligionEnum.Other;
                default:
                    return ReligionEnum.Islam;
            }
        }
    }
}
