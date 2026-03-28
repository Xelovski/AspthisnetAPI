using BussinessLayer.Interfaces.Services;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiCController : ControllerBase
    {
        private readonly IUserService _userService;

        public ApiCController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet(Name ="GetUsers")]
        [SwaggerOperation(Summary = "Get all users")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var o = await _userService.GetByIdAsync(id);
            if (o == null)
            {
                return NotFound();//too lazy to add more messages... :(
            }
            return Ok(o);
        }
        [HttpPost(Name ="AddUser")]
        public async Task<IActionResult> CreateAsync([FromBody] cREATEuSERModel us)
        {
            if (us == null)
            {
                return BadRequest("Missing");
            }
            var ot = 0; var q = new UserDTO();
            var userList = await _userService.GetAllAsync();
            foreach (var user in userList) { if (ot < user.Id) { ot = user.Id; } }
            if (us == null || us.Name == "" || us.Email == "" || us.Name == null || us.Email == null)
            { q = new UserDTO() { Name = "wh", Emil = "q@q.q", Id = ot + 1, PublicId = Guid.NewGuid() }; }
            else { q = new UserDTO() { Name = us.Name, Emil = us.Email, Id = ot + 1, PublicId = Guid.NewGuid() }; }
            await _userService.CreateAsync(q);
            var l = new LoginDTO();
            if (us == null || us.Name == "" || us.Password == null || us.Password == "" || us.Name == null)
            {
                l = new LoginDTO() { Name = "wh", Password = "0", PublicId = Guid.NewGuid() };
            }
            else
            {
                l = new LoginDTO() { Name = us.Name, Password = us.Password, PublicId = Guid.NewGuid() };
            }
            return Ok(await _userService.Register(l));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateModel model)
        {
            if (model == null || id == 0)
            {
                return BadRequest("Something is missing. . .");
            }
            var o = await _userService.GetByIdAsync(id);
            var user = new UserDTO() { Id = id, Name = o.Name, PublicId = o.PublicId, Emil = model.Email };
            var updated = await _userService.UpdateAsync(user);
            if (!updated)
                return NotFound("Hold up how did ya get here?");
            return Ok(updated);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id = 0)
        {
            if (id == 0)
            {
                return BadRequest("User of ID 0 does not exist (or if not given)");
            }
            var o = await _userService.GetByIdAsync(id);
            var deleted = await _userService.DeleteAsync(o.PublicId);
            if (!deleted)
                return NotFound("The user does not exist");
            return Ok(deleted);
        }
        [HttpPost("login")]//Login
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO login)
        {
            if (login == null)
            {
                return BadRequest("Missing login info");
            }
            else
            {
                var q = await _userService.LoginAsync(login);
                if (!q)
                {
                    return BadRequest("Wrong name or password");
                }
                return Ok(q);
            }
        }
    }
}