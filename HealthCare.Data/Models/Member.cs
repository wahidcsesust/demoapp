using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Data.Models
{
    public class Member : BaseEntity
    {
        public int RegNo { get; set; }

        [Required]
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string ImageName { get; set; }
        public byte[] ImageData { get; set; }
        public string FatherName { get; set; }
        public string BloodGroup { get; set; }
        public string Profession { get; set; }
        public string MotherName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [NotMapped]
        public string DateOfBirthString { get { return DateOfBirth.HasValue ? DateOfBirth.Value.ToString("dd-MM-yyyy") : string.Empty; } }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string NationalIdNo { get; set; }
        public string PassportNo { get; set; }
        public string AreaLocation { get; set; }
        public string Category { get; set; }
        public string Designation { get; set; }
        public string NomineeName { get; set; }
        public string NomineeRelation { get; set; }
        public string NomineeNationalIdNo { get; set; }
        public int? NomineeAge { get; set; }


        [NotMapped]
        public string BloodGroupString
        {
            get
            {
                if (BloodGroup == "APositive")
                {
                    return "A +ve";
                }
                else if (BloodGroup == "BPostive")
                {
                    return "B +ve";
                }
                else if (BloodGroup == "ABPositive")
                {
                    return "AB +ve";
                }
                else if (BloodGroup == "OPositive")
                {
                    return "O +ve";
                }
                else if (BloodGroup == "ANegative")
                {
                    return "A -ve";
                }
                else if (BloodGroup == "BNegative")
                {
                    return "B -ve";
                }
                else if (BloodGroup == "ABNegative")
                {
                    return "AB -ve";
                }
                else if (BloodGroup == "ONegative")
                {
                    return "O -ve";
                }
                else
                {
                    return BloodGroup;
                }

            }
        }
        [NotMapped]
        public string AreaLocationString
        {
            get
            {
                if (AreaLocation == "East")
                {
                    return "Purbo Para";
                }
                else if (AreaLocation == "West")
                {
                    return "Poschim Para";
                }
                else if (AreaLocation == "North")
                {
                    return "Uttor Para";
                }
                else if (AreaLocation == "South")
                {
                    return "Dokkhin Para";
                }
                else if (AreaLocation == "Other")
                {
                    return "Other";
                }
                else
                {
                    return "";
                }
            }

        }

        [NotMapped]
        public string CategoryString
        {
            get
            {
                if (Category == "AdvisoryCouncil")
                {
                    return "Advisory Council";
                }
                else if (Category == "ExecutiveCouncil")
                {
                    return "Executive Council";
                }
                else if (Category == "GeneralMember")
                {
                    return "General Member";
                }
                else if (Category == "Other")
                {
                    return "Other";
                }
                else
                {
                    return Category;
                }
            }
        }
        [NotMapped]
        public string DesignationString
        {
            get
            {
                if (Designation == "Advisor")
                {
                    return "Advisor";
                }
                else if (Designation == "President")
                {
                    return "President";
                }
                else if (Designation == "VicePresident")
                {
                    return "Vice President";
                }
                else if (Designation == "GeneralSecretary")
                {
                    return "General Secretary";
                }
                else if (Designation == "JointSecretary")
                {
                    return "Joint Secretary";
                }
                else if (Designation == "OrganizationalSecretary")
                {
                    return "Organizational Secretary";
                }
                else if (Designation == "ViceOrganizationalSecretary")
                {
                    return "Vice Organizational Secretary";
                }
                else if (Designation == "PublicitySecretary")
                {
                    return "Publicity Secretary";
                }
                else if (Designation == "VicePublicitySecretary")
                {
                    return "Vice Publicity Secretary";
                }
                else if (Designation == "FinanceSecretary")
                {
                    return "Finance Secretary";
                }
                else if (Designation == "SocialWelfareSecretary")
                {
                    return "Social Welfare Secretary";
                }
                else if (Designation == "SportsAndCultureSecretary")
                {
                    return "Sports And Culture Secretary";
                }
                else if (Designation == "OfficeSecretary")
                {
                    return "Office Secretary";
                }
                else if (Designation == "ViceOfficeSecretary")
                {
                    return "Vice Office Secretary";
                }
                else if (Designation == "ReligiousAffairsSecretary")
                {
                    return "Religious Affairs Secretary";
                }
                else if (Designation == "LegalSecretary")
                {
                    return "Legal Secretary";
                }
                else if (Designation == "WomenAffairsSecretary")
                {
                    return "Women Affairs Secretary";
                }
                else if (Designation == "EnvironmentalSecretary")
                {
                    return "Environmental Secretary";
                }
                else if (Designation == "LibraryAffairsSecretary")
                {
                    return "Library Affairs Secretary";
                }
                else if (Designation == "HealthAffairsSecretary")
                {
                    return "Health Affairs Secretary";
                }
                else if (Designation == "Other")
                {
                    return "Other";
                }
                else
                {
                    return Designation;
                }
            }
        }
    }
}
