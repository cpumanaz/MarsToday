using MarsToday.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MarsToday.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ILogger _logger;

        private List<Photo> allPhotos = new List<Photo>();

        private string[] dateFormats = { "MM/dd/yy", "MMMM d, yyyy", "MMM-dd-yyyy" };

        private NasaPhotos photosJson = new NasaPhotos();

        private List<DateTime> validatedDates = new List<DateTime>();

        public HomeController(IHostingEnvironment hostingEnvironment, ILogger<HomeController> logger, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "A brief description of MarsToday";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact Matt Shooshtari";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Photos()
        {
            string baseURL = _configuration.GetValue<string>("RoverAPI:APIURI");
            string apiVersion = _configuration.GetValue<string>("RoverAPI:APIversion");
            string rover = _configuration.GetValue<string>("RoverAPI:RoverName");
            string camera = _configuration.GetValue<string>("RoverAPI:Camera");
            string apiKey = _configuration.GetValue<string>("RoverAPI:APIKey");
            IEnumerable<String> dateList = System.IO.File.ReadLines(_hostingEnvironment.WebRootPath + "\\App_Data\\DateList.txt");

            foreach (string date in dateList)
            {
                try
                {
                    var validDate = DateTime.ParseExact(date, dateFormats, new CultureInfo("en-US"), DateTimeStyles.None);
                    validatedDates.Add(validDate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, date + " is not a valid DateTime");
                }
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                foreach (DateTime date in validatedDates)
                {
                    string imageDate = date.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en-US"));
                    Uri uri = new Uri($"{baseURL}/{apiVersion}/rovers/{rover}/photos?earth_date={imageDate}&camera={camera}&api_key={apiKey}");
                    HttpResponseMessage Res = await client.GetAsync(uri).ConfigureAwait(false);

                    if (Res.IsSuccessStatusCode)
                    {
                        var response = Res.Content.ReadAsStringAsync().Result;
                        photosJson = JsonConvert.DeserializeObject<NasaPhotos>(response);
                        allPhotos.AddRange(photosJson.photos);
                    }
                }
                return View(allPhotos);
            }
        }
    }
}