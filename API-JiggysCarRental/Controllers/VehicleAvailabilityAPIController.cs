using API_JiggysCarRental.DATA;
using API_JiggysCarRental.MODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_JiggysCarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class VehicleAvailabilityAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _cxt;

        public VehicleAvailabilityAPIController(ApplicationDbContext cxt)
        {
            this._cxt = cxt;
        }


        //VEHICLE AVAILABILY ENDPOINTS
        [HttpGet("Availability")]
        public IActionResult GetAvailabilities()
        {
            var availability = _cxt.Availabilites.Include(a => a.Vehicle).ToList();

            if (availability == null)
            {
                return BadRequest();
            }
            return Ok(availability);
        }


        [HttpGet("{id}")]
        public IActionResult GetAvailabilityById(int id)
        {
            var availability = _cxt.Availabilites.Include(a => a.Vehicle).FirstOrDefault(v => v.Id == id);

            if (availability == null)
            {
                return NotFound();
            }
            return Ok(availability);
        }

        //Create
        [HttpPost("AvailabilityPost")]
        public IActionResult CreateAvailability([FromBody] Availability values)
        {
            _cxt.Availabilites.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetAvailabilityById), new { id = values.Id }, values);
        }

        //Edit
        [HttpPut("AvailabilityPut")]

        public IActionResult UpdateRecord([FromBody] Availability values)
        {
            if (values.Id == null)
            {
                return NotFound();
            }
            _cxt.Availabilites.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetAvailabilityById), new {id = values.Id }, values);
        }




        //Delete
        [HttpDelete("{id}")]
        public IActionResult DeleteAvailabilityById(int id)
        {
            var availability = _cxt.Availabilites.Include(a => a.Vehicle).FirstOrDefault(x =>x.Id == id);

            if (availability == null)
            {
                return NotFound();
            }
            _cxt.Availabilites.Remove(availability);
            _cxt.SaveChanges();
            return Ok(availability);
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
