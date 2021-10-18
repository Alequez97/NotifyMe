using DomainEntites;
using LinkLookupSubscriptionApi.Models;
using LinkLookupSubscriptionApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LinkLookupSubscriptionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private IDataRepository<Group> _groupsDataRepository;

        public GroupsController(IDataRepositoryFactory dataRepositoryFactory)
        {
            _groupsDataRepository = dataRepositoryFactory.Get<Group>("Groups");
        }
        
        // GET: api/<ValuesController>
        [HttpGet]
        public ActionResult<List<Group>> Get()
        {
            var groups = _groupsDataRepository.ReadAll();
            return groups != null ? Ok(groups) : NotFound();
        }

        //GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public ActionResult<Group> Get(string id)
        {
            var group = _groupsDataRepository.Read(id);
            return group != null ? Ok(group) : NotFound();
        }

        [HttpGet("get-by-name/{groupName}")]
        public ActionResult<Group> GetByUsername(string groupName)
        {
            var group = _groupsDataRepository.FindByExpression(x => x.Name == groupName);
            return group != null ? Ok(group) : NotFound();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] Group group)
        {
            var isSuccess = _groupsDataRepository.Write(group);
            return isSuccess ? Ok() : new JsonResult(new Response() { Message = "Failed to save user" }) { StatusCode = StatusCodes.Status500InternalServerError };
        }

        //PUT api/<ValuesController>
        [HttpPut]
        public IActionResult Put([FromBody] Group group)
        {
            var isSuccess = _groupsDataRepository.Update(group);
            return isSuccess ? Ok() : new JsonResult(new Response() { Message = "Failed to update user" } ) { StatusCode = StatusCodes.Status500InternalServerError };
        }

        //// DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var isSuccess = _groupsDataRepository.Delete(id);
            return isSuccess ? Ok() : new JsonResult(new Response() { Message = "Failed to delete user" } ) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}
