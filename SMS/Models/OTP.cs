using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace SMS.Models
{
    public class OTP
    {
        public static string SentOTP(string sURL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);
            request.MaximumAutomaticRedirections = 4;
            request.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string sResponse = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                return sResponse;
            }
            catch(Exception ex)
            {
                return "";
            }


        }


        public void SendOTP(string toNumber, string messageBody)
        {
            //Your code goes here
            String url = "https://my.forwardvaluesms.com/vendorsms/pushsms.aspx?&";
            String result = "";
            String message = HttpUtility.UrlEncode(messageBody);
            String strPost = "apikey=b7d8297f-9435-4c12-b19f-ce3221f92212&clientid=a56b9346-7003-4575-a3d9-d8cb9c1c9cb3&msisdn=" + toNumber + "&sid=Uni Solz&msg=" + message + "&fl=0";
            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(strPost);
            }
            catch (Exception ex)
            {
                //logger.ErrorException(ex.Message, ex);
            }
            finally
            {
                myWriter.Close();
            }
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                // Close and clean up the StreamReader 
                sr.Close();
            }
        }

    }
}