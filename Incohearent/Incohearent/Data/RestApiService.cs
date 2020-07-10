using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;



namespace Incohearent.Data
{
    public class RestApiService
    {
        private HttpClient client;
        //string grant_type = "password";
        

        public RestApiService()
        {
            /*
            
            Konstruktor za REST servis. 
            > Inicijalizira se HTTP klijent
            > Postavi se maksimalna količina bajtova za istovremeno procesiranje
            > Klijent prihvaća zahtjeve sa headerom definiranim kao [application/x-www-form-urlencoded]
               
            */

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        }

        public string GetPublicIpAddress()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://ifconfig.me");
            request.UserAgent = "curl"; // Simulira linux curl naredbu - prijenos podataka
            string pubAddr;
            request.Method = "GET";
            using (WebResponse response = request.GetResponse()) { 
                using (var reader=new StreamReader(response.GetResponseStream()))
                {
                    pubAddr = reader.ReadToEnd();
                }
            }

            return pubAddr;
        }
    }
}
