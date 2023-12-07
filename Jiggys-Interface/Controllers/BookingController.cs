using Jiggys_Interface.Models;
using Jiggys_Interface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Jiggys_Interface.Controllers
{
    public class BookingController : Controller
    {
       
        const string BOOKING_BASE_URL = "https://localhost:7028/api/BookingAPI";
        const string CUSTOMER_ENDPOINT = "Customer";
        const string VEHICLE_ENDPOINT = "Vehicle";

        const string BOOKING_ADDON_BASE_URL = "https://localhost:7028/api/BookingAddOnAPI";
        const string ADDON_ENDPOINT = "AddOn";




        //INDEX 1 
        public IActionResult BookingVMIndex()
        {
            var bookingVMlist = new List<BookingVM>();
          
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BOOKING_BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                

                HttpResponseMessage response1 = client.GetAsync($"{BOOKING_BASE_URL}/Booking").Result;

                HttpResponseMessage response2 = client.GetAsync($"{BOOKING_ADDON_BASE_URL}/AddOn").Result;



                if (response1.IsSuccessStatusCode)
                {
                    var data1 = response1.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    bookingVMlist = JsonConvert.DeserializeObject<List<BookingVM>>(data1);



                    if (response2.IsSuccessStatusCode)
                    {
                        var data2 = response1.Content.ReadAsStringAsync().Result;
                        //Deserialise object from Json string
                        bookingVMlist = JsonConvert.DeserializeObject<List<BookingVM>>(data2);
                    }


                }
            }

            return View(bookingVMlist);
           
        }




        //INDEX 2
        public IActionResult Index()
        {
            //Booked, Succes Message, From Booking Controller
            if (TempData.ContainsKey("message"))
            {
                ViewData["message"] = TempData["message"].ToString();
            }//-------------


            var bookinglist = new List<Booking>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BOOKING_BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response1 = client.GetAsync($"{BOOKING_BASE_URL}/Booking").Result;

                if (response1.IsSuccessStatusCode)
                {
                    var data1 = response1.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    bookinglist = JsonConvert.DeserializeObject<List<Booking>>(data1);
                }
            }
            return View(bookinglist);
        }





        //-----------------------------------------------
        //CREATE: GET
        [HttpGet]
        public IActionResult Create()
        {
            Booking booking = new Booking(); //Global

            //lists
            List<Customer> customerList = new List<Customer>();
            List<Vehicle> vehicleList = new List<Vehicle>();
            List<AddOn> addOnList = new List<AddOn>();  



            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BOOKING_BASE_URL}"); ///
                client.BaseAddress = new Uri($"{BOOKING_ADDON_BASE_URL}"); ///

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //Customer
                HttpResponseMessage cusResponse = client.GetAsync($"{BOOKING_BASE_URL}/{CUSTOMER_ENDPOINT}").Result;
                if (cusResponse.IsSuccessStatusCode)
                {
                    var cusData = cusResponse.Content.ReadAsStringAsync().Result;
                    customerList = JsonConvert.DeserializeObject<List<Customer>>(cusData)!;
                }


                //Vehicle
                HttpResponseMessage vehResponse = client.GetAsync($"{BOOKING_BASE_URL}/{VEHICLE_ENDPOINT}").Result;
                if (vehResponse.IsSuccessStatusCode)
                {
                    var vehData = vehResponse.Content.ReadAsStringAsync().Result;
                    vehicleList = JsonConvert.DeserializeObject<List<Vehicle>>(vehData)!;
                }


                //AddOnn - uses diffrent api controller url
                HttpResponseMessage addOnResponse = client.GetAsync($"{BOOKING_ADDON_BASE_URL}/{ADDON_ENDPOINT}").Result;
                if (addOnResponse.IsSuccessStatusCode)
                {
                    var addOnData = addOnResponse.Content.ReadAsStringAsync().Result;
                    addOnList = JsonConvert.DeserializeObject<List<AddOn>>(addOnData)!;
                }

                //for DDL
                var viewModel = new BookingVM
                {
                    //Customer
                    CustomerList = customerList.Select(cus => new SelectListItem
                    {
                        Text = cus.FullName,
                        Value = cus.Id.ToString(),
                    }).ToList(),

                    //Vehicle
                    VehicleList = vehicleList.Select(vehicleList => new SelectListItem
                    {
                        Text = vehicleList.VehicleName,
                        Value = vehicleList.Id.ToString(),
                    }).ToList(),


                    //AddOn 
                    AddOnList = addOnList.Select(addOnList => new SelectListItem
                    {
                        Text = addOnList.AddOnName,
                        Value = addOnList.Id.ToString(),
                    }).ToList(),
                    
                };
                return View(viewModel);

            }

        }


        //CREATE:POST
        [HttpPost]
        public async Task <IActionResult> Create(BookingVM bookingVm, int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BOOKING_BASE_URL}"); ///
            client.BaseAddress = new Uri($"{BOOKING_ADDON_BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var booking = new Booking
            {
                Id = id,
                CustomerId = bookingVm.SelectedCustomerId,
                VehicleId = bookingVm.SelectedVehicleId,
                StartDate = bookingVm.Booking.StartDate,
                EndDate= bookingVm.Booking.EndDate,
                PickUpLocation = bookingVm.Booking.PickUpLocation,
                DropOffLocation= bookingVm.Booking.DropOffLocation,
                TotalPrice= bookingVm.Booking.TotalPrice,
                isPaid = bookingVm.Booking.isPaid,  
                 
            };


            //Saves bookingId & AddonnId into BookingAddon
            var booking2 = new BookingAddOn
            {
                Id = id,

                AddOnnId = bookingVm.SelectedAddOnId,
                BookingId = booking.Id,
                Booking = booking,
            };



            //booking
            //var json1 = JsonConvert.SerializeObject(booking);
            //var data1 = new StringContent(json1, System.Text.Encoding.UTF8, "application/json");
            //HttpResponseMessage bookResponse = client.PostAsync($"{BOOKING_BASE_URL}/BookingPost", data1).Result; 

            //bookingAddon
            var json2 = JsonConvert.SerializeObject(booking2);
            var data2 = new StringContent(json2, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage bookingAddonResponse = client.PostAsync($"{BOOKING_ADDON_BASE_URL}/BookingPost", data2).Result; 



            if (bookingAddonResponse.IsSuccessStatusCode)
            {

                TempData["message"] = "Vehicle Successfully Booked"; // Succes message displayed in index
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Create Booking");
                return View(bookingVm);
            }

        }


        //----------------------------------------------------------------------
        //EDIT:GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Booking booking = new Booking(); //Global
            BookingAddOn bookingAddOn = new BookingAddOn();

            //lists
            List<Customer> customerList = new List<Customer>();
            List<Vehicle> vehicleList = new List<Vehicle>();
            List<AddOn> addOnList = new List<AddOn>();



            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BOOKING_BASE_URL}"); ///
                client.BaseAddress = new Uri($"{BOOKING_ADDON_BASE_URL}"); ///

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //Customer
                HttpResponseMessage cusResponse = client.GetAsync($"{BOOKING_BASE_URL}/{CUSTOMER_ENDPOINT}").Result;
                if (cusResponse.IsSuccessStatusCode)
                {
                    var cusData = cusResponse.Content.ReadAsStringAsync().Result;
                    customerList = JsonConvert.DeserializeObject<List<Customer>>(cusData)!;
                }


                //Vehicle
                HttpResponseMessage vehResponse = client.GetAsync($"{BOOKING_BASE_URL}/{VEHICLE_ENDPOINT}").Result;
                if (vehResponse.IsSuccessStatusCode)
                {
                    var vehData = vehResponse.Content.ReadAsStringAsync().Result;
                    vehicleList = JsonConvert.DeserializeObject<List<Vehicle>>(vehData)!;
                }


                //AddOnn - uses diffrent api controller url
                HttpResponseMessage addOnResponse = client.GetAsync($"{BOOKING_ADDON_BASE_URL}/{ADDON_ENDPOINT}").Result;
                if (addOnResponse.IsSuccessStatusCode)
                {
                    var addOnData = addOnResponse.Content.ReadAsStringAsync().Result;
                    addOnList = JsonConvert.DeserializeObject<List<AddOn>>(addOnData)!;
                }



                //Booking List
                HttpResponseMessage bookingResponse = client.GetAsync($"{BOOKING_BASE_URL}/{id}").Result;
                if(bookingResponse.IsSuccessStatusCode)
                {
                    var bookingData = bookingResponse.Content.ReadAsStringAsync().Result;
                    booking = JsonConvert.DeserializeObject<Booking>(bookingData)!;
                }

                //

                


                //for DDL
                var viewModel = new BookingVM
                {
                    Id = booking.Id,
                    SelectedCustomerId = booking.CustomerId,
                    SelectedVehicleId = booking.VehicleId,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,
                    PickUpLocation = booking.PickUpLocation,
                    DropOffLocation = booking.DropOffLocation,
                    TotalPrice = booking.TotalPrice,
                    isPaid = booking.isPaid,

                    //////
                   
                    SelectedAddOnId = bookingAddOn.AddOnnId,
                    //Id = bookingAddOn.BookingId,
                    Booking = booking,
               




                //Customer
                CustomerList = customerList.Select(cus => new SelectListItem
                    {
                        Text = cus.FullName,
                        Value = cus.Id.ToString(),
                    }).ToList(),

                    //Vehicle
                    VehicleList = vehicleList.Select(vehicleList => new SelectListItem
                    {
                        Text = vehicleList.VehicleName,
                        Value = vehicleList.Id.ToString(),
                    }).ToList(),


                    //AddOn 
                    AddOnList = addOnList.Select(addOnList => new SelectListItem
                    {
                        Text = addOnList.AddOnName,
                        Value = addOnList.Id.ToString(),
                    }).ToList(),

                };
                return View(viewModel);

            }

        }





        //EDIT:POST
        [HttpPost]
        public  IActionResult Edit(BookingVM bookingVm)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BOOKING_BASE_URL}"); ///
            client.BaseAddress = new Uri($"{BOOKING_ADDON_BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var booking = new Booking
            {
                Id = bookingVm.Id,
                CustomerId = bookingVm.SelectedCustomerId,
                VehicleId = bookingVm.SelectedVehicleId,
                StartDate = bookingVm.StartDate,
                EndDate = bookingVm.EndDate,
                PickUpLocation = bookingVm.PickUpLocation,
                DropOffLocation = bookingVm.DropOffLocation,
                TotalPrice = bookingVm.TotalPrice,
                isPaid = bookingVm.isPaid,

            };


            //Saves bookingId & AddonnId into BookingAddon
            var booking2 = new BookingAddOn
            {
                Id = bookingVm.Id,

                AddOnnId = bookingVm.SelectedAddOnId,
                BookingId = booking.Id,
                Booking = booking,
            };


            //booking
            var json1 = JsonConvert.SerializeObject(booking);
            var data1 = new StringContent(json1, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage bookResponse = client.PutAsync($"{BOOKING_BASE_URL}/BookingPut", data1).Result;

            ////bookingAddon
            //var json2 = JsonConvert.SerializeObject(booking2);
            //var data2 = new StringContent(json2, System.Text.Encoding.UTF8, "application/json");
            //HttpResponseMessage bookingAddonResponse = client.PostAsync($"{BOOKING_ADDON_BASE_URL}/BookingPost", data2).Result;



            if (bookResponse.IsSuccessStatusCode)
            {

                TempData["message"] = "Booking Successfully Updated"; // Succes message displayed in index
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Update Booking");
                return View(bookingVm);
            }
        }




         //-----------------------------------
        // DETAIL:GET 
        public IActionResult Detail(int id)
        {
            var booking = new Booking();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BOOKING_BASE_URL}/{id}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{BOOKING_BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    booking = JsonConvert.DeserializeObject<Booking>(data);
                }
            }
            return View(booking);
        }



        //try out
        public IActionResult JustAddonDetail(int id)
        {
            var addedOn = new BookingAddOn();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BOOKING_ADDON_BASE_URL}/{id}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{BOOKING_ADDON_BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    addedOn = JsonConvert.DeserializeObject<BookingAddOn>(data);
                }
            }
            return View(addedOn);
        }



         //---------------------------------------------------------------
        //DELETE: GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            
            Booking booking = new Booking();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BOOKING_BASE_URL}/{id}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{BOOKING_BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    booking = JsonConvert.DeserializeObject<Booking>(data);
                }
            }
            return View(booking);
        }



        //DELETE: POST
        public IActionResult Delete(int id, Booking booking)
        {

            //Booking booking = new Booking();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BOOKING_BASE_URL}/{id}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync($"{BOOKING_BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    booking = JsonConvert.DeserializeObject<Booking>(data);
                }
            }
            TempData["message"] = "Booking Deleted"; // Succes message displayed in index
            return RedirectToAction("Index");
        }
    }
}
