using LinkLookupSubscriptionApi.Models;
using LinkLookupSubscriptionApi.Services;
using LinkLookupSubscriptionApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LinkLookupSubscriptionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IDataRepository<User> _userRepository;

        public UsersController(IDataRepositoryFactory dataRepositoryFactory)
        {
            _userRepository = dataRepositoryFactory.Create<User>("Users");
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        public ActionResult<Group> Post([FromBody] User user)
        {
            var success = _userRepository.Write(user);
            if (success)
            {
                return new JsonResult($"User {user.Username} successfully added") { StatusCode = StatusCodes.Status201Created };
            }
            else
            {
                return new JsonResult($"User {user.Username} wasn't successfully added") { StatusCode = StatusCodes.Status500InternalServerError };
            }

        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
