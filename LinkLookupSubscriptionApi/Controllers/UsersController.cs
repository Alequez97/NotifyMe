using LinkLookupSubscriptionApi.Models;
using LinkLookupSubscriptionApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LinkLookupSubscriptionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IDataRepository<User> _dataRepository;

        public UsersController(IDataRepositoryFactory dataRepositoryFactory)
        {
            _dataRepository = dataRepositoryFactory.Get<User>("Users");
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            var users = _dataRepository.ReadAll();
            if (users == null || users.Count == 0)
            {
                return new JsonResult($"Fail to find") { StatusCode = StatusCodes.Status404NotFound };
            }

            return new JsonResult(users);
        }

        // GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [Route("/username")]
        [HttpGet]
        public ActionResult<User> Get([FromQuery] string username)
        {
            var user = _dataRepository.FindByExpression(x => x.Username == username);
            if (user == null)
            {
                return new JsonResult($"Fail to find") { StatusCode = StatusCodes.Status404NotFound };
            }

            return new JsonResult(user);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            var success = _dataRepository.Write(user);
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
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
