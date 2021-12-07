using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Services
{
    public static class Constants
    {
        public static string ConnectionString { get; set; } = "Server=172.106.164.8,1090;Initial Catalog=healthcare;Persist Security Info=True;User ID=codebondsbd;Password=codeBonds@2018;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30"; //"Server=DESKTOP-LBHSL4C\\SQLEXPRESS;Database=HC;Trusted_Connection=True;MultipleActiveResultSets=true;";

        public const string LangCookieKey = "lang";
        public const string DateFormat = "dd-MM-yyyy";
        public const string DateTimeFormat = "yyyy-MM-dd hh:mm:ss";

        //public static string ClientId = "9d0bd7f7-ba24-450e-9c31-0d01eff54b97";
        //public static string Secret = "F5B8n7h-aFNaxRm-pt7.tofDyjE=Mrvl";
        //public static string TenantId = "70d22a8d-923a-445e-82d4-32329da21746";

        public static string ClientId = "2d43d684-50be-43a9-8645-968c6e21b714";
        public static string Secret = "ZbaP8YXPeoLB3sVO8YFJ1/OU0DxLiNNcW2PlRNKef0k=";
        public static string TenantId = "5972a6b1-1b82-4865-a158-809d4fd719a3";
        public static string UserId = "korjournal@marbit.se";

        public const string GlobalDeleteMessage = "Data deleted successfully";
        public const string GlobalSuccessMessage = "Data sparas framgångsrikt";// Data saved successfully
        public const string GlobalErrorMessage = "Ett fel uppstod, data som inte sparades."; //An error occurred, data not saved.

    }
}
