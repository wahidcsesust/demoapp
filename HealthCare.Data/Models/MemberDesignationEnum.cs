using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthCare.Data.Models
{
    public enum MemberDesignationEnum
    {
        [Display(Name = "Advisor")]
        Advisor = 1,
        [Display(Name = "President")]
        President = 2,
        [Display(Name = "Vice President")]
        VicePresident = 3,
        [Display(Name = "General Secretary")]
        GeneralSecretary = 4,
        [Display(Name = "Joint Secretary")]
        JointSecretary = 5,
        [Display(Name = "Organizational Secretary")]
        OrganizationalSecretary = 6,
        [Display(Name = "Vice Organizational Secretary")]
        ViceOrganizationalSecretary = 7,
        [Display(Name = "Publicity Secretary")]
        PublicitySecretary = 8,
        [Display(Name = "Vice Publicity Secretary")]
        VicePublicitySecretary = 9,
        [Display(Name = "Finance Secretary")]
        FinanceSecretary = 10,
        [Display(Name = "Social Welfare Secretary")]
        SocialWelfareSecretary = 11,
        [Display(Name = "Sports and Culture Secretary")]
        SportsAndCultureSecretary = 12,
        [Display(Name = "Office Secretary")]
        OfficeSecretary = 13,
        [Display(Name = "Vice Office Secretary")]
        ViceOfficeSecretary = 14,
        [Display(Name = "Religious Affairs Secretary")]
        ReligiousAffairsSecretary = 15,
        [Display(Name = "Legal Secretary")]
        LegalSecretary = 16,
        [Display(Name = "Women's Affairs Secretary")]
        WomenAffairsSecretary = 17,
        [Display(Name = "Environmental Secretary")]
        EnvironmentalSecretary = 18,
        [Display(Name = "Library Affairs Secretary")]
        LibraryAffairsSecretary = 19,
        [Display(Name = "Health Affairs Secretary")]
        HealthAffairsSecretary = 20,
        [Display(Name = "Member")]
        Member = 21,
        [Display(Name = "Other")]
        Other = 22,
    }
}
