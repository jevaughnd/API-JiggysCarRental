using API_JiggysCarRental.DATA;
using API_JiggysCarRental.MODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_JiggysCarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _cxt;
        public BookingAPIController(ApplicationDbContext context)
        {
            this._cxt = context;
        }

        //BOOKING ENDPOINTS
        [HttpGet("Booking")]
        public IActionResult GetBookings()
        {
            var booking = _cxt.Bookings.Include(b=> b.Customer)
                                       .Include(b=> b.Vehicle)
                                       .ToList();
            if(booking == null)
            {
                return BadRequest();
            }
            return Ok(booking);
        }

        //Find Individual Booking Record By Id
        [HttpGet("{id}")]
        public IActionResult GetBookingById(int id)
        {
            var booking = _cxt.Bookings.Include(b => b.Customer)
                                        .Include(b=> b.Vehicle)
                                        .FirstOrDefault(x => x.Id == id);
            if(booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }


        //Create Record
        [HttpPost("BookingPost")]
        public IActionResult CreateBooking([FromBody] Booking values )
        {
            _cxt.Bookings.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetBookingById), new { id = values.Id }, values);
        }


        //Edit Record
        [HttpPut("BookingPut")]
        public IActionResult UpdateBooking([FromBody] Booking bookingValues )
        {
            if (bookingValues.Id == null)
            {
                return NotFound();
            }
            _cxt.Bookings.Update(bookingValues);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetBookingById), new {id = bookingValues.Id }, bookingValues);
        }


        //Delete Individual Record By id
        [HttpDelete("{id}")]
        public IActionResult DeleteBookingById(int id)
        {
            var booking = _cxt.Bookings.Include(b => b.Customer)
                                        .Include(b => b.Vehicle)
                                        .FirstOrDefault(x => x.Id == id);

            if (booking == null)
            {
                return NotFound();
            }
            _cxt.Bookings.Remove(booking);
            _cxt.SaveChanges();
            return Ok(booking);
        }






        //=========================================================================================================================//
        //CUSTOMER ENPOINTS

        [HttpGet("Customer")]
        public IActionResult GetCustomers()
        {
            var persons = _cxt.Customers.ToList();

            if (persons == null)
            {
                return BadRequest();
            }
            return Ok(persons);
        }


        //Finds Individual Customer By Id
        [HttpGet("Customer/{Id}")]
        public IActionResult GetCustomerById(int id)
        {
            var person = _cxt.Customers.FirstOrDefault(c => c.Id == id);

            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }



        //Create
        [HttpPost("CustomerPost")]
        public IActionResult CreateCustomer([FromBody] Customer values)
        {
            _cxt.Customers.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCustomerById), new { id = values.Id }, values);
        }


        //Edit 
        [HttpPut("CustomerPut")]
        public IActionResult UpdateCustomer(int id, [FromBody] Customer values)
        {
            var customer = _cxt.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            _cxt.Customers.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCustomerById), new { id = values.Id }, values);
        }








        //=========================================================================================================================//
        //VEHICLE ENDPOINTS

        [HttpGet("Vehicle")]
        public IActionResult GetVehicles()
        {
            var vehicle = _cxt.Vehicles.ToList();
            if (vehicle == null)
            {
                return BadRequest();
            }

            return Ok(vehicle);
        }


        //Find Individual Vehicle By id
        [HttpGet("Vehicle/{Id}")]
        public IActionResult GetVehicleById(int id)
        {
            var vehicle = _cxt.Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }


        //Create 
        [HttpPost("VehiclePost")]
        public IActionResult CreateVehicle([FromBody] Vehicle values)
        {
            _cxt.Vehicles.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetVehicleById), new { id = values.Id }, values);
        }




        //Edit 
        [HttpPut("VehiclePut")]
        public IActionResult UpdateVehicle(int id, [FromBody] Vehicle vehicleValues)
        {
            var vehicle = _cxt.Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            _cxt.Vehicles.Update(vehicleValues);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetVehicleById), new { id = vehicleValues.Id }, vehicleValues);

        }
    }
}
