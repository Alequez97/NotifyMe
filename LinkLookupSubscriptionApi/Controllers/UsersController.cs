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
    public class UsersController : ControllerBase
    {
        private IDataRepository<User> _usersDataRepository;
        private readonly IMapper _mapper;

        public UsersController(IDataRepositoryFactory dataRepositoryFactory, IMapper mapper)
        {
            _usersDataRepository = dataRepositoryFactory.Get<User>("Users");
            _mapper = mapper;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public ActionResult<List<UserDto>> Get()
        {
            var users = _usersDataRepository.ReadAll();
            var usersDto = _mapper.Map<List<UserDto>>(users);
            return usersDto != null ? Ok(usersDto) : NotFound();
        }

        //GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public ActionResult<UserDto> Get(string id)
        {
            var user = _usersDataRepository.Read(id);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto != null ? Ok(userDto) : NotFound();
        }

        [HttpGet("get-by-username/{username}")]
        public ActionResult<UserDto> GetByUsername(string username)
        {
            var user = _usersDataRepository.FindByExpression(x => x.Username == username);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto != null ? Ok(userDto) : NotFound();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var isSuccess = _usersDataRepository.Write(user);
            return isSuccess ? Ok() : new JsonResult(new Response() { Message = "Failed to save user" }) { StatusCode = StatusCodes.Status500InternalServerError };
        }

        //PUT api/<ValuesController>
        [HttpPut]
        public IActionResult Put([FromBody] UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
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
