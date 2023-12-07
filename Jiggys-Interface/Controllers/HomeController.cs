using Jiggys_Interface.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Jiggys_Interface.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

       

        //This Shows The Gallary Images In Index
        public IActionResult Index()
        {
            var vehicleList = new List<Vehicle>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/Vehicle").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    vehicleList = JsonConvert.DeserializeObject<List<Vehicle>>(data);
                }
            }
            return View(vehicleList);
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

        const string BASE_URL = "https://localhost:7028/api/VehicleAPI";

        //INDEX
        //this shows the inventory Images
        public IActionResult Inventory()
        {
            var vehicleList = new List<Vehicle>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/Vehicle").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    vehicleList = JsonConvert.DeserializeObject<List<Vehicle>>(data);
                }
            }
            return View(vehicleList);
        }


        // DETAIL pic:GET 
        //this shows the details of the inventory
        public IActionResult VehicleDetail(int id)
        {
            var vehicle = new Vehicle();

            using (HttpClient client = new HttpClient())
            {
                //client.BaseAddress = new Uri(BASE_URL);
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    vehicle = JsonConvert.DeserializeObject<Vehicle>(data);
                }
            }
            return View(vehicle);
        }




        const string Availability_URL = "https://localhost:7028/api/VehicleAvailabilityAPI";

        //DETAIL:GET Availability //---------------------------------------------------------
        public IActionResult AvailabilityDetail(int id)
        {
            Availability availability = new Availability();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Availability_URL}/{id}"); ///string interpulation

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = client.GetAsync($"{Availability_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    availability = JsonConvert.DeserializeObject<Availability>(data);
                }
            }
            return View(availability);

        } 

        
        public IActionResult BookedDetail(int id)
        {
            const string BOOKING_URL = "https://localhost:7028/api/BookingAPI";
            // DETAIL:GET  //checks if booked

            Booking booking = new Booking();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BOOKING_URL}/{id}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{BOOKING_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    booking = JsonConvert.DeserializeObject<Booking>(data);
                }
            }
            return View(booking);
        }

    }
}
