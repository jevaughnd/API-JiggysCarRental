using Jiggys_Interface.Models;
using Jiggys_Interface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;

namespace Jiggys_Interface.Controllers
{
    public class AvailabilityController : Controller
    {

        const string Availability_BASE_URL = "https://localhost:7028/api/VehicleAvailabilityAPI";

        const string Vehicle_End_point = "Vehicle";


        //INDEX
        public IActionResult Index() // When Makeing vehicle availability status manully select each vehicle in order,
                                     // acording to the drop down list,
        {

            var availabilityList = new List<Availability>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Availability_BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{Availability_BASE_URL}/Availability").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    availabilityList = JsonConvert.DeserializeObject<List<Availability>>(data);
                }
            }
            return View(availabilityList);
        }


         //---------------------------
        //CREATE: GET
        [HttpGet] 
        public IActionResult Create()
        {
            Availability availability = new Availability();

            List<Vehicle> vehicleList = new List<Vehicle>();


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Availability_BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //vehicle list
                HttpResponseMessage vehResponse = client.GetAsync($"{Availability_BASE_URL}/{Vehicle_End_point}").Result;
                if (vehResponse.IsSuccessStatusCode)
                {
                    var vehData = vehResponse.Content.ReadAsStringAsync().Result;
                    vehicleList = JsonConvert.DeserializeObject<List<Vehicle>>(vehData)!;
                }


                var viewModel = new AvailabilityVM
                {
                    VehicleList = vehicleList.Select(x => new SelectListItem
                    {
                        Text = x.VehicleName,
                        Value = x.Id.ToString(),

                    }).ToList(),

                };

                return View(viewModel);
            }

        }


        //Create:post
        [HttpPost]
        public IActionResult Create(AvailabilityVM availabilityVM, int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{Availability_BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var av = new Availability
            {
                Id = id,
                AvailabilityDate = availabilityVM.AvailabilityDate,
                VehicleId = availabilityVM.SelectedVehicleId,
                IsAvailable = availabilityVM.IsAvailable,
            };

            var json = JsonConvert.SerializeObject(av);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage aResponse = client.PostAsync($"{Availability_BASE_URL}/AvailabilityPost", data).Result; ///Updates use put request 
            if (aResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to create availability");
                return View(availabilityVM);
            }
        }




         //--------------------------------------------------
        //EDIT:GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Availability availability = new Availability();

            List<Vehicle> vehicleList = new List<Vehicle>();


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Availability_BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //vehicle list
                HttpResponseMessage vehResponse = client.GetAsync($"{Availability_BASE_URL}/{Vehicle_End_point}").Result;
                if (vehResponse.IsSuccessStatusCode)
                {
                    var vehData = vehResponse.Content.ReadAsStringAsync().Result;
                    vehicleList = JsonConvert.DeserializeObject<List<Vehicle>>(vehData)!;
                }

                //Availbilites {id}
                HttpResponseMessage AvailabilityResponse = client.GetAsync($"{Availability_BASE_URL}/{id}").Result;
                if (AvailabilityResponse.IsSuccessStatusCode)
                {
                    var avData = AvailabilityResponse.Content.ReadAsStringAsync().Result;
                    availability = JsonConvert.DeserializeObject<Availability>(avData)!;
                }

              

                var viewModel = new AvailabilityVM
                {
                    Id = id,
                    AvailabilityDate = availability.AvailabilityDate,
                    SelectedVehicleId = availability.VehicleId,
                    IsAvailable = availability.IsAvailable,

                    VehicleList = vehicleList.Select(x => new SelectListItem
                    {
                        Text = x.VehicleName,
                        Value = x.Id.ToString(),

                    }).ToList(),

                };

                return View(viewModel);
            }

        }



        //EDIT:POST
        [HttpPost]
        public IActionResult Edit(AvailabilityVM availabilityVM)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{Availability_BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var av = new Availability
            {
                Id = availabilityVM.Id,
                AvailabilityDate = availabilityVM.AvailabilityDate,
                VehicleId = availabilityVM.SelectedVehicleId,
                IsAvailable = availabilityVM.IsAvailable,
            };

            var json = JsonConvert.SerializeObject(av);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage aResponse = client.PutAsync($"{Availability_BASE_URL}/AvailabilityPut", data).Result; ///Updates use put request 
            if (aResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable To Update Availability");
                return View(availabilityVM);
            }

        }





        //DETAIL:GET //----------------------------------------
        public IActionResult Detail(int id)
        {
            Availability availability = new Availability();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Availability_BASE_URL}/{id}"); ///string interpulation

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = client.GetAsync($"{Availability_BASE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    availability = JsonConvert.DeserializeObject<Availability>(data);
                }
            }
            return View(availability);

        } 



        //Delete:GET //---------------------------------------------------------
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Availability availability = new Availability();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Availability_BASE_URL}/{id}"); ///string interpulation

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = client.GetAsync($"{Availability_BASE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    availability = JsonConvert.DeserializeObject<Availability>(data);
                }
            }
            return View(availability);

        }


        //Delete Post
        [HttpPost]
        public IActionResult Delete(int id, Availability availability)
        {
            //Availability availability = new Availability();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Availability_BASE_URL}/{id}"); ///string interpulation

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = client.DeleteAsync($"{Availability_BASE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    availability = JsonConvert.DeserializeObject<Availability>(data);
                }
            }
            return RedirectToAction("Index");

        }
    }
}
