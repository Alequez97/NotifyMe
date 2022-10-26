using CommonUtils.Interfaces;
using CommonUtils.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LinkLookupSubscriptionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private IBackgroundServiceMiddleware _backgroundServiceMiddleware;

        public ServicesController(IBackgroundServiceMiddleware backgroundServiceMiddleware)
        {
            _backgroundServiceMiddleware = backgroundServiceMiddleware;
        }

        //GET api/<ValuesController>/serviceName
        [HttpGet("{name}")]
        public ActionResult<ServiceStatus> Get(string name)
        {
            return _backgroundServiceMiddleware.GetServiceStatus(name);
        }

        //[HttpGet("get-by-name/{groupName}")]
        //public ActionResult<Group> GetByUsername(string groupName)
        //{
        //    var group = _groupsDataRepository.FindByExpression(x => x.Name == groupName);
        //    return group != null ? Ok(group) : NotFound();
        //}

        //// POST api/<ValuesController>
        //[HttpPost]
        //public IActionResult Post([FromBody] Group group)
        //{
        //    var isSuccess = _groupsDataRepository.Write(group);
        //    return isSuccess ? Ok() : new JsonResult(new Response() { Message = "Failed to save user" }) { StatusCode = StatusCodes.Status500InternalServerError };
        //}

        ////PUT api/<ValuesController>
        //[HttpPut]
        //public IActionResult Put([FromBody] Group group)
        //{
        //    var isSuccess = _groupsDataRepository.Update(group);
        //    return isSuccess ? Ok() : new JsonResult(new Response() { Message = "Failed to update user" }) { StatusCode = StatusCodes.Status500InternalServerError };
        //}

        ////// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public IActionResult Delete(string id)
        //{
        //    var isSuccess = _groupsDataRepository.Delete(id);
        //    return isSuccess ? Ok() : new JsonResult(new Response() { Message = "Failed to delete user" }) { StatusCode = StatusCodes.Status500InternalServerError };
        //}
    }
}
