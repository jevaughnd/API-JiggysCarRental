using API_JiggysCarRental.DATA;
using API_JiggysCarRental.MODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_JiggysCarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingAddOnAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _cxt;
        public BookingAddOnAPIController(ApplicationDbContext context)
        {
            this._cxt = context;
        }

        //BOOKING ENDPOINTS
        [HttpGet("Booking")]
        public IActionResult GetBookings()
        {
            var booking = _cxt.Bookings.ToList();

            if (booking == null)
            {
                return BadRequest();
            }
            return Ok(booking);
        }

        //Find Individual Booking Record By Id
        [HttpGet("Booking/{id}")]
        public IActionResult GetBookingById(int id)
        {
            var booking = _cxt.Bookings.FirstOrDefault(x => x.Id == id);

            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }


        //Create Record // This saves the bookingId + AddonId into BookingAddon
        [HttpPost("BookingPost")]
        public IActionResult CreateBooking([FromBody] BookingAddOn values)
        {
            _cxt.Bookings.Add(values.Booking);
            _cxt.SaveChanges();
            values.BookingId = values.Booking.Id;    
            _cxt.BookingAddOns.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetBookingById), new { id = values.Id }, values);
        }


        ////Edit Record
        //[HttpPut("BookingPut")]
        //public IActionResult UpdateBooking([FromBody] Booking bookingValues)
        //{
        //    if (bookingValues.Id == null)
        //    {
        //        return NotFound();
        //    }
        //    _cxt.Bookings.Update(bookingValues);
        //    _cxt.SaveChanges();
        //    return CreatedAtAction(nameof(GetBookingById), new { id = bookingValues.Id }, bookingValues);
        //}

        //Edit Record
        [HttpPut("BookingPut")]
        public IActionResult UpdateBooking([FromBody] Booking bookingValues)
        {
            if (bookingValues.Id == null)
            {
                return NotFound();
            }
            _cxt.Bookings.Update(bookingValues);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetBookingById), new { id = bookingValues.Id }, bookingValues);
        }






        //=========================================================================================================================//



        //ADDON ENDPOINTS

        [HttpGet("AddOn")]
        public IActionResult GetAddOn()
        {
            var addOns = _cxt.AddOns.ToList();
            if(addOns == null)
            {
                return BadRequest();
            }
            return Ok(addOns);
        }

        //Find
        [HttpGet("AddOn/{Id}")]
        public IActionResult GetAddOnById(int id)
        {
            var addOn = _cxt.AddOns.FirstOrDefault(a => a.Id == id);
            if (addOn ==null)
            {
                return NotFound();
            }
            return Ok(addOn);
        }

        //create
        [HttpPost("AddOnPost")]
        public IActionResult CreateAddOn([FromBody] AddOn addOns)
        {
            _cxt.AddOns.Add(addOns);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetAddOnById), new { id = addOns.Id }, addOns);
        }
    }
}
