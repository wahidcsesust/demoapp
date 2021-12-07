using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Services
{
    public class SmsService : ISmsService
    {
        public bool SendSms(string mobileNumbers, string messageText)
        {
            bool isSendSuccess = false;

            String userid = "islampursms"; //Your Login ID
            String password = "Islampurwelfare@2021"; //Your Password
                                                      //Recipient Phone Number multiple number must be separated by comma
            String message = System.Uri.EscapeUriString(messageText);

            // Create a request using a URL that can receive a post.   
            WebRequest request = WebRequest.Create("http://66.45.237.70/api.php");
            // Set the Method property of the request to POST.  
            request.Method = "POST";
            // Create POST data and convert it to a byte array.  
            string postData = "username=" + userid + "&password=" + password + "&number=" + mobileNumbers + "&message=" + messageText;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.  
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.  
            request.ContentLength = byteArray.Length;
            // Get the request stream.  
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.  
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.  
            dataStream.Close();
            // Get the response.  
            WebResponse response = request.GetResponse();

            string status = ((HttpWebResponse)response).StatusDescription;


            // Display the status.  
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.  
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.  
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();
            // Display the content.  
            //Console.WriteLine(responseFromServer);
            // Clean up the streams.  
            reader.Close();
            dataStream.Close();


            response.Close();
            isSendSuccess = status == "OK";
            return isSendSuccess;
        }
    }
}
