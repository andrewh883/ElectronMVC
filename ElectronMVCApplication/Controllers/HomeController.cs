using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ElectronMVCApplication.Models;
using ElectronMVCApplication.Data;
using Microsoft.PowerPlatform.Cds.Client;
using System.Security;
using System.Net;
using System.Net.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace ElectronMVCApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext dbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            //var test = dbContext.Database.CanConnect();
            //return Json(test);
            //return View();
            string serviceUrl = "https://yourorg.crm.dynamics.com";
            string clientId = "a17caa81-8704-49e8-8125-68d85e5fec3d";
            string userName = "andrewh@notalvision.com";
            string password = "&gxzK,6A2jMJ9m/";

            AuthenticationContext authContext =
                new AuthenticationContext("https://login.microsoftonline.com/common", false);
            UserCredential credential = new UserCredential(userName);
            AuthenticationResult result = authContext.AcquireTokenAsync(serviceUrl, clientId, credential).Result;
            //The access token
            string accessToken = result.AccessToken;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(serviceUrl);
                client.Timeout = new TimeSpan(0, 2, 0);  //2 minutes  
                client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request =
                    new HttpRequestMessage(HttpMethod.Get, "/api/data/v9.0/WhoAmI");
                //Set the access token
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = client.SendAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    //Get the response content and parse it.  
                    JObject body = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    Guid userId = (Guid)body["UserId"];
                    Console.WriteLine("Your system user ID is: {0}", userId);
                }

                return View();
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
