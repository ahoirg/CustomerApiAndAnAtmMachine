using CustomerApi.Interfaces;
using CustomerApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CustomerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IBLCustomer _blcustomer;
        public CustomerController(IBLCustomer blcustomer)
        {
            _blcustomer = blcustomer;
        }

        [HttpPost(Name = "PostCustomer")]
        public IActionResult Post(MCustomer[] customers)
        {
            var toReturn = _blcustomer.AddCustomer(customers.ToList());
            return Ok(toReturn);
        }

        [HttpGet(Name = "GetCustomer")]
        public IActionResult Get()
        {
            var toReturn = _blcustomer.GetAllCustomer();
            return Ok(toReturn);
        }
    }
}
