using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment4_NASA.DataAccess;
using Assignment4_NASA.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Assignment4_NASA.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext dbContext;
        string BASE_URL = "https://api.nasa.gov/planetary";
        static string API_KEY = "GfN38vq7pmbaJXjgoYtSk2aDQxhZewdeRmIrzMBH"; //Add your API key here inside ""
        HttpClient httpClient;

        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(new
                System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<Apod> GetApod()
        {
            string APOD_API_PATH = BASE_URL + "/apod?api_key="+API_KEY;
            string apodList = "";
            List<Apod> apod = null;

            // connect to the IEXTrading API and retrieve information
            httpClient.BaseAddress = new Uri(APOD_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(APOD_API_PATH).GetAwaiter().GetResult();

            // read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                apodList = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // now, parse the Json strings as C# objects
            if (!apodList.Equals(""))
            {
                
                apod = JsonConvert.DeserializeObject<List<Apod>>(apodList);
                apod = apod.GetRange(0, 50);
            }

            return apod;
        }

        public IActionResult Index()
        {
            //Set ViewBag variable first
            ViewBag.dbSuccessComp = 0;
            List<Apod> apod = GetApod();

            //Save companies in TempData, so they do not have to be retrieved again
            TempData["Apods"] = JsonConvert.SerializeObject(apod);

            return View(apod);
        }

        public IActionResult Apods()
        {
            //Set ViewBag variable first
            ViewBag.dbSuccessComp = 0;
            List<Apod> apod = GetApod();

            //Save companies in TempData, so they do not have to be retrieved again
            TempData["Apods"] = JsonConvert.SerializeObject(apod);

            return View(apod);
        }

        public IActionResult PopulateApod()
        {
            // Retrieve the companies that were saved in the symbols method
            List<Apod> apod = JsonConvert.DeserializeObject<List<Apod>>(TempData["Apods"].ToString());

            foreach (Apod apods in apod)
            {
                //Database will give PK constraint violation error when trying to insert record with existing PK.
                //So add company only if it doesnt exist, check existence using symbol (PK)
                if (dbContext.apod.Where(c => c.date.Equals(apods.date)).Count() == 0)
                {
                    dbContext.apod.Add(apods);
                }
            }

            dbContext.SaveChanges();
            ViewBag.dbSuccessComp = 1;
            return View("Index", apod);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
