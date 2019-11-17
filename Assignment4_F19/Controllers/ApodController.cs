using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Assignment4_F19.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment4_F19.Controllers
{
    public class ApodController : Controller
    {
        // Obtaining the API key is easy. The same key should be usable across the entire
        // data.gov developer network, i.e. all data sources on data.gov.
        // https://www.nps.gov/subjects/developer/get-started.htm

        static string BASE_URL = "https://api.nasa.gov/planetary/";
        static string API_KEY = "GfN38vq7pmbaJXjgoYtSk2aDQxhZewdeRmIrzMBH"; //Add your API key here inside ""

        HttpClient httpClient;

        /// <summary>
        ///  Constructor to initialize the connection to the data source
        /// </summary>
        public ApodController()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Method to receive data from API end point as a collection of objects
        /// 
        /// JsonConvert parses the JSON string into classes
        /// </summary>
        /// <returns></returns>
        public Apod GetApod()
        {
            string APOD_API_PATH = BASE_URL + "/apod?api_key=" + API_KEY;
            string ApodData = "";

            Apod apod = null;

            httpClient.BaseAddress = new Uri(APOD_API_PATH);

            // It can take a few requests to get back a prompt response, if the API has not received
            //  calls in the recent past and the server has put the service on hibernation
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(APOD_API_PATH).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    ApodData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!ApodData.Equals(""))
                {
                    // JsonConvert is part of the NewtonSoft.Json Nuget package
                    apod = JsonConvert.DeserializeObject<Apod>(ApodData);
                }
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message
                Console.WriteLine(e.Message);
            }

            return apod;
        }

        
    }
}