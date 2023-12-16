using Jiggys_Interface.Models;
using Jiggys_Interface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Jiggys_Interface.Controllers
{
    public class VehicleController : Controller
    {
        const string BASE_URL = "https://localhost:7028/api/VehicleAPI";

        const string VEHICLECATEGORY_ENDPOINT = "VehicleCategory";


        //INDEX
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








        //------------------------------------------------
        //CREATE:GET
        [HttpGet]
        public IActionResult Create()
        {
            Vehicle vehicle = new Vehicle();//global variable

            List<VehicleCategory> categoryList = new List<VehicleCategory>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //VehicleCategory
                HttpResponseMessage catResponse = client.GetAsync($"{BASE_URL}/{VEHICLECATEGORY_ENDPOINT}").Result;
                if (catResponse.IsSuccessStatusCode)
                {
                    var catData = catResponse.Content.ReadAsStringAsync().Result;
                    categoryList = JsonConvert.DeserializeObject<List<VehicleCategory>>(catData)!;
                }

                //DDL in VM
                var viewModel = new VehicleVM
                {
                    //VehicleCategory
                    VehicleCategoryList = categoryList.Select(Vclist => new SelectListItem
                    {
                        Text = Vclist.CategoryName,
                        Value = Vclist.Id.ToString(),

                    }).ToList(),
                };
                return View(viewModel);
            }
        }


        //CREATE:POST
        [HttpPost]
        public async Task<IActionResult> Create(VehicleVM vehicleVm, int id)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            //file upload
            if (!ModelState.IsValid) return View(vehicleVm);


            var vehicle = new VehicleCreateDTO
            {
                VehicleCategoryId = vehicleVm.SelectedVehicleCategoryId,
                VehicleName = vehicleVm.VehicleName,
                VehicleDescription = vehicleVm.VehicleDescription,
                LicencePlateData = vehicleVm.LicencePlateData,

                FrontImgFile = vehicleVm.FrontImgFile,
                SideImgFile = vehicleVm.SideImgFile,
                InteriorImgFile = vehicleVm.InteriorImgFile,

                NumberOf_Passengers = vehicleVm.NumberOf_Passengers,
                NumberOf_LargeBags = vehicleVm.NumberOf_LargeBags,

                Has_AirConditioner = vehicleVm.Has_AirConditioner,
                Has_Radio_CDPlayer = vehicleVm.Has_Radio_CDPlayer,
                Has_PowerSteering_Windows = vehicleVm.Has_PowerSteering_Windows,
                Has_AutomaticTransmission = vehicleVm.Has_AutomaticTransmission,
            };


            var response = await SendVehicleToAPI(vehicle);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Create Vehicle");
                return View(vehicleVm);
            }
        }


        //CREATE: file POST
        private async Task<HttpResponseMessage> SendVehicleToAPI(VehicleCreateDTO vehicleDTO)
        {
            using (var httpClient = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    // Set the Content-Type header to "multipart/form-data"
                    formData.Headers.ContentType.MediaType = "multipart/form-data";

                    // Add student data to the request
                    formData.Add(new StringContent(vehicleDTO.VehicleCategoryId.ToString()), "VehicleCategoryId");
                    formData.Add(new StringContent(vehicleDTO.VehicleName.ToString()), "VehicleName");
                    formData.Add(new StringContent(vehicleDTO.VehicleDescription.ToString()), "VehicleDescription");
                    formData.Add(new StringContent(vehicleDTO.LicencePlateData.ToString()), "LicencePlateData");

                    formData.Add(new StringContent(vehicleDTO.NumberOf_Passengers.ToString()), "NumberOf_Passengers");
                    formData.Add(new StringContent(vehicleDTO.NumberOf_LargeBags.ToString()), "NumberOf_LargeBags");

                    formData.Add(new StringContent(vehicleDTO.Has_AirConditioner.ToString()), "Has_AirConditioner");
                    formData.Add(new StringContent(vehicleDTO.Has_Radio_CDPlayer.ToString()), "Has_Radio_CDPlayer");
                    formData.Add(new StringContent(vehicleDTO.Has_PowerSteering_Windows.ToString()), "Has_PowerSteering_Windows");
                    formData.Add(new StringContent(vehicleDTO.Has_AutomaticTransmission.ToString()), "Has_AutomaticTransmission");


                    //Front img           Add the file to the request
                    if (vehicleDTO.FrontImgFile != null && vehicleDTO.FrontImgFile.Length > 0)
                    {
                        formData.Add(new StreamContent(vehicleDTO.FrontImgFile.OpenReadStream())
                        {
                            Headers = { ContentLength = vehicleDTO.FrontImgFile.Length,
                                ContentType = new MediaTypeHeaderValue(
                                    vehicleDTO.FrontImgFile.ContentType)
                            }

                        }, "FrontImgFile", vehicleDTO.FrontImgFile.FileName);


                        //Side img
                        if (vehicleDTO.SideImgFile != null && vehicleDTO.SideImgFile.Length > 0)
                        {
                            formData.Add(new StreamContent(vehicleDTO.SideImgFile.OpenReadStream())
                            {
                                Headers = { ContentLength = vehicleDTO.SideImgFile.Length,
                                    ContentType = new MediaTypeHeaderValue(
                                        vehicleDTO.SideImgFile.ContentType)
                            }

                            }, "SideImgFile", vehicleDTO.SideImgFile.FileName);


                            //Interior img
                            if (vehicleDTO.InteriorImgFile != null && vehicleDTO.InteriorImgFile.Length > 0)
                            {

                                formData.Add(new StreamContent(vehicleDTO.InteriorImgFile.OpenReadStream())
                                {
                                    Headers = { ContentLength = vehicleDTO.InteriorImgFile.Length,
                                        ContentType = new MediaTypeHeaderValue(
                                            vehicleDTO.InteriorImgFile.ContentType)
                                    }

                                }, "InteriorImgFile", vehicleDTO.InteriorImgFile.FileName);

                            }//3

                        }//2

                    }//1


                    //Send to API Code
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

                    // Send the data to the API
                    return await httpClient.PostAsync($"{BASE_URL}/filePost", formData);
                    
                }
            }
        }









        //------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------


        //------------------------------------------------
        //EDIT FILE:GET
        [HttpGet]
        public IActionResult EditFile(int id)
        {
            Vehicle vehicle = new Vehicle();//global variable
            VehicleCreateDTO createDTO = new VehicleCreateDTO();
            List<VehicleCategory> categoryList = new List<VehicleCategory>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //VehicleCategory
                HttpResponseMessage catResponse = client.GetAsync($"{BASE_URL}/{VEHICLECATEGORY_ENDPOINT}").Result;
                if (catResponse.IsSuccessStatusCode)
                {
                    var catData = catResponse.Content.ReadAsStringAsync().Result;
                    categoryList = JsonConvert.DeserializeObject<List<VehicleCategory>>(catData)!;
                }

                //Vehicle id
                HttpResponseMessage vehRes = client.GetAsync($"{BASE_URL}/{id}").Result; // {id} shows values in form
                if (vehRes.IsSuccessStatusCode)
                {
                    var data = vehRes.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    vehicle = JsonConvert.DeserializeObject<Vehicle>(data)!;
                }




                //DDL in VM
                var viewModel = new VehicleVM
                {
                    Id = vehicle.Id,
                    SelectedVehicleCategoryId = vehicle.VehicleCategoryId,
                    VehicleName = vehicle.VehicleName,
                    VehicleDescription = vehicle.VehicleDescription,
                    LicencePlateData = vehicle.LicencePlateData,

                    FrontImgFile = createDTO.FrontImgFile,
                    SideImgFile = createDTO.SideImgFile,
                    InteriorImgFile = createDTO.InteriorImgFile,

                    NumberOf_Passengers = vehicle.NumberOf_Passengers,
                    NumberOf_LargeBags = vehicle.NumberOf_LargeBags,
                    Has_AirConditioner = vehicle.Has_AirConditioner,
                    Has_Radio_CDPlayer = vehicle.Has_Radio_CDPlayer,
                    Has_PowerSteering_Windows = vehicle.Has_PowerSteering_Windows,
                    Has_AutomaticTransmission = vehicle.Has_AutomaticTransmission,

                    //VehicleCategory
                    VehicleCategoryList = categoryList.Select(Vclist => new SelectListItem
                    {
                        Text = Vclist.CategoryName,
                        Value = Vclist.Id.ToString(),

                    }).ToList(),

                };
                return View(viewModel);

            }
        }







        //EDIT: FILE Post
        [HttpPost]
        public async Task<IActionResult> EditFile(VehicleVM vehicleVm)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            //file upload
            if (!ModelState.IsValid) return View(vehicleVm);


            var vehicle = new VehicleCreateDTO
            {
               
                VehicleCategoryId = vehicleVm.SelectedVehicleCategoryId,
                VehicleName = vehicleVm.VehicleName,
                VehicleDescription = vehicleVm.VehicleDescription,
                LicencePlateData = vehicleVm.LicencePlateData,

                FrontImgFile = vehicleVm.FrontImgFile,
                SideImgFile = vehicleVm.SideImgFile,
                InteriorImgFile = vehicleVm.InteriorImgFile,

                NumberOf_Passengers = vehicleVm.NumberOf_Passengers,
                NumberOf_LargeBags = vehicleVm.NumberOf_LargeBags,

                Has_AirConditioner = vehicleVm.Has_AirConditioner,
                Has_Radio_CDPlayer = vehicleVm.Has_Radio_CDPlayer,
                Has_PowerSteering_Windows = vehicleVm.Has_PowerSteering_Windows,
                Has_AutomaticTransmission = vehicleVm.Has_AutomaticTransmission,
            };




            var response = await SendVehicleToAPI2(vehicle, vehicleVm.Id);

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Update Vehicle");
                return View(vehicleVm);
            }
        }


        //EDIT: FILE PUT
        private async Task<HttpResponseMessage> SendVehicleToAPI2(VehicleCreateDTO vehicleDTO, int id = 0) // id to edit id
        {
            const string Get_Vehicles = "https://localhost:7028/api/VehicleAPI/get-vehicle-images/"; //not used

            using (var httpClient = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    // Set the Content-Type header to "multipart/form-data"
                    formData.Headers.ContentType.MediaType = "multipart/form-data";

                    // Add vehicle data to the request
                    formData.Add(new StringContent(vehicleDTO.VehicleCategoryId.ToString()), "VehicleCategoryId");
                    formData.Add(new StringContent(vehicleDTO.VehicleName.ToString()), "VehicleName");
                    formData.Add(new StringContent(vehicleDTO.VehicleDescription.ToString()), "VehicleDescription");
                    formData.Add(new StringContent(vehicleDTO.LicencePlateData.ToString()), "LicencePlateData");

                    formData.Add(new StringContent(vehicleDTO.NumberOf_Passengers.ToString()), "NumberOf_Passengers");
                    formData.Add(new StringContent(vehicleDTO.NumberOf_LargeBags.ToString()), "NumberOf_LargeBags");

                    formData.Add(new StringContent(vehicleDTO.Has_AirConditioner.ToString()), "Has_AirConditioner");
                    formData.Add(new StringContent(vehicleDTO.Has_Radio_CDPlayer.ToString()), "Has_Radio_CDPlayer");
                    formData.Add(new StringContent(vehicleDTO.Has_PowerSteering_Windows.ToString()), "Has_PowerSteering_Windows");
                    formData.Add(new StringContent(vehicleDTO.Has_AutomaticTransmission.ToString()), "Has_AutomaticTransmission");


                    //Front img                 Add the file to the request
                    if (vehicleDTO.FrontImgFile != null && vehicleDTO.FrontImgFile.Length > 0)
                    {
                        formData.Add(new StreamContent(vehicleDTO.FrontImgFile.OpenReadStream())
                        {
                            Headers = { ContentLength = vehicleDTO.FrontImgFile.Length,
                                ContentType = new MediaTypeHeaderValue(
                                    vehicleDTO.FrontImgFile.ContentType)
                            }

                        }, "FrontImgFile", vehicleDTO.FrontImgFile.FileName);


                        //Side img
                        if (vehicleDTO.SideImgFile != null && vehicleDTO.SideImgFile.Length > 0)
                        {
                            formData.Add(new StreamContent(vehicleDTO.SideImgFile.OpenReadStream())
                            {
                                Headers = { ContentLength = vehicleDTO.SideImgFile.Length,
                                    ContentType = new MediaTypeHeaderValue(
                                        vehicleDTO.SideImgFile.ContentType)
                            }

                            }, "SideImgFile", vehicleDTO.SideImgFile.FileName);

                            //Interior img
                            if (vehicleDTO.InteriorImgFile != null && vehicleDTO.InteriorImgFile.Length > 0)
                            {

                                formData.Add(new StreamContent(vehicleDTO.InteriorImgFile.OpenReadStream())
                                {
                                    Headers = { ContentLength = vehicleDTO.InteriorImgFile.Length,
                                        ContentType = new MediaTypeHeaderValue(
                                            vehicleDTO.InteriorImgFile.ContentType)
                                    }

                                }, "InteriorImgFile", vehicleDTO.InteriorImgFile.FileName);

                            }//3

                        }//2

                    }//1
             
                    //Send to API Code
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

                    // Send the data to the API
                    return await httpClient.PutAsync($"{BASE_URL}/filePut/{id}", formData); 
                }
            }
        }


        //---------------------------------------------------------------------------------------------
        //test out to delete img files + record

        // DELETE: GET
        [HttpGet]
        public IActionResult DeleteVehicle(int id)
        {
            Vehicle vehicle = new Vehicle();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    // Deserialise object from Json string
                    vehicle = JsonConvert.DeserializeObject<Vehicle>(data);
                }
            }

            return View(vehicle);
        }
        


        //DELETE: FILE: POST
       [HttpPost]
        public IActionResult DeleteVehicleConfirmed(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/deleteVehicle/{id}");
               

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync($"{BASE_URL}/deleteVehicle/{id}").Result;
               // HttpResponseMessage response = await client.DeleteAsync($"{BASE_URL}/deleteVehicle/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle error, e.g., show an error message to the user
                    ModelState.AddModelError(string.Empty, "Unable to delete vehicle and files.");
                    return RedirectToAction("EditFile", new { id });
                }
            }
        }



        //-------
        //------------------------------------------------------------------------------



        //-----------------------------------------------------------------------------
        // DETAIL:GET 
        public IActionResult Detail(int id)
        {
            var vehicle = new Vehicle();

            using (HttpClient client = new HttpClient())
            {
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

        // DETAIL pic:GET 
        public IActionResult DetailWithPic(int id)
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








        //--------------------------------------------------------------------
        [HttpGet]
        //DELETE:GET
        public IActionResult Delete(int id)
        {
            Vehicle vehicle = new Vehicle();

            using (HttpClient client = new HttpClient())
            {
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


        [HttpPost]
        //DELETE:POST
        public IActionResult Delete(int id, Vehicle vehicle)
        {
            //var vehicle = new Vehicle();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync($"{BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    vehicle = JsonConvert.DeserializeObject<Vehicle>(data);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
