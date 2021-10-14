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
        private IDataRepository<Group> _usersDataRepository;
        private readonly IMapper _mapper;

        public GroupsController(IDataRepositoryFactory dataRepositoryFactory, IMapper mapper)
        {
            _usersDataRepository = dataRepositoryFactory.Get<Group>("Groups");
            _mapper = mapper;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public ActionResult<List<GroupDto>> Get()
        {
            var users = _usersDataRepository.ReadAll();
            var usersDto = _mapper.Map<List<GroupDto>>(users);
            return usersDto != null ? Ok(usersDto) : NotFound();
        }

        //GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public ActionResult<GroupDto> Get(string id)
        {
            var user = _usersDataRepository.Read(id);
            var userDto = _mapper.Map<GroupDto>(user);
            return userDto != null ? Ok(userDto) : NotFound();
        }

        [HttpGet("get-by-username/{username}")]
        public ActionResult<GroupDto> GetByUsername(string username)
        {
            var user = _usersDataRepository.FindByExpression(x => x.Name == username);
            var userDto = _mapper.Map<GroupDto>(user);
            return userDto != null ? Ok(userDto) : NotFound();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] GroupDto userDto)
        {
            var user = _mapper.Map<Group>(userDto);
            var isSuccess = _usersDataRepository.Write(user);
            return isSuccess ? Ok() : new JsonResult(new Response() { Message = "Failed to save user" }) { StatusCode = StatusCodes.Status500InternalServerError };
        }

        //PUT api/<ValuesController>
        [HttpPut]
        public IActionResult Put([FromBody] GroupDto userDto)
        {
            var user = _mapper.Map<Group>(userDto);
            var isSuccess = _usersDataRepository.Update(user);
            return isSuccess ? Ok() : new JsonResult(new Response() { Message = "Failed to update user" } ) { StatusCode = StatusCodes.Status500InternalServerError };
        }

        //// DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var isSuccess = _usersDataRepository.Delete(id);
            return isSuccess ? Ok() : new JsonResult(new Response() { Message = "Failed to delete user" } ) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}
