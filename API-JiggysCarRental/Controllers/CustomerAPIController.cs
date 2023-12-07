using API_JiggysCarRental.DATA;
using API_JiggysCarRental.MODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_JiggysCarRental.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _cxt;

        public CustomerAPIController(ApplicationDbContext context)
        {
            this._cxt = context;
        }


        //CUSTOMER END POINTS

        [HttpGet("Customer")]
        public IActionResult GetCustomers()
        {
            var persons = _cxt.Customers.Include(p => p.Parish).ToList();

            if (persons == null)
            {
                return BadRequest();
            }
            return Ok(persons);
        }


        //Finds Individual Record By Id
        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            var person = _cxt.Customers.Include(p => p.Parish).FirstOrDefault(x => x.Id == id);

            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }


        //Create Record
        [HttpPost("CustomerPost")]
        public IActionResult CreateCustomer([FromBody] Customer values)
        {
            _cxt.Customers.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCustomerById), new {id = values.Id},values);
        }


        //Edit Record
        [HttpPut("CustomerPut")]
        public IActionResult UpdateCustomer([FromBody] Customer values)
        {
            if(values.Id == null)
            {
                return NotFound();
            }
            _cxt.Customers.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCustomerById), new {id = values.Id},values);
        }




        //Delete Individual Record By Id
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomerById(int id)
        {
            var person = _cxt.Customers.Include(p => p.Parish).FirstOrDefault(x=> x.Id ==id);

            if (person == null)
            {
                return NotFound();
            }
            _cxt.Customers.Remove(person);
            _cxt.SaveChanges();
            return Ok(person);
        }



        //=========================================================================================================================//
        //PARISH END POINTS

        //-----------------------
        [HttpGet]
        [Route("Parish")]
        public IActionResult GetParishes()
        {
            var parItem = _cxt.Parishes.ToList();
            if (parItem == null)
            {
                return BadRequest();
            }
            return Ok(parItem);
        }

        //-----------------------
        //Finds Record where id is = to the result of the firstOrDefault query
        [HttpGet]
        [Route("Parish/{Id}")]
        public IActionResult GetParishById(int id)
        {
            var parItem = _cxt.Parishes.FirstOrDefault(x => x.Id == id); //gets individual parish by Id
            if (parItem == null)
            {
                return NotFound();
            }
            return Ok(parItem);
        }

        //-----------------------
        [HttpPost]
        [Route("ParishPost")]
        public IActionResult CreateParish([FromBody] Parish values)
        {
            _cxt.Parishes.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetParishById), new { id = values.Id }, values);
        }

        //-----------------------
        [HttpPut]
        [Route("ParishPut")]
        public IActionResult UpdateParish(int id, [FromBody] Parish values)
        {
            var parItem = _cxt.Parishes.FirstOrDefault(x => x.Id ==id);
            if (parItem == null)
            {
                return NotFound();
            }
            _cxt.Parishes.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetParishById), new { id = values.Id }, values);
        }
    }
}
