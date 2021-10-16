using AutoMapper;
using LinkLookupSubscriptionApi.Models;
using LinkLookupSubscriptionApi.Models.DTO;
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
        private readonly IMapper _mapper;

        public GroupsController(IDataRepositoryFactory dataRepositoryFactory, IMapper mapper)
        {
            _groupsDataRepository = dataRepositoryFactory.Get<Group>("Groups");
            _mapper = mapper;
        }
        
        // GET: api/<ValuesController>
        [HttpGet]
        public ActionResult<List<GroupDto>> Get()
        {
            var groups = _groupsDataRepository.ReadAll();
            var groupsDto = _mapper.Map<List<GroupDto>>(groups);
            return groupsDto != null ? Ok(groupsDto) : NotFound();
        }

        //GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public ActionResult<GroupDto> Get(string id)
        {
            var group = _groupsDataRepository.Read(id);
            var groupDto = _mapper.Map<GroupDto>(group);
            return groupDto != null ? Ok(groupDto) : NotFound();
        }

        [HttpGet("get-by-username/{username}")]
        public ActionResult<GroupDto> GetByUsername(string groupName)
        {
            var group = _groupsDataRepository.FindByExpression(x => x.Name == groupName);
            var groupDto = _mapper.Map<GroupDto>(group);
            return groupDto != null ? Ok(groupDto) : NotFound();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] GroupDto groupDto)
        {
            var group = _mapper.Map<Group>(groupDto);
            var isSuccess = _groupsDataRepository.Write(group);
            return isSuccess ? Ok() : new JsonResult(new Response() { Message = "Failed to save user" }) { StatusCode = StatusCodes.Status500InternalServerError };
        }

        //PUT api/<ValuesController>
        [HttpPut]
        public IActionResult Put([FromBody] GroupDto groupDto)
        {
            var group = _mapper.Map<Group>(groupDto);
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
