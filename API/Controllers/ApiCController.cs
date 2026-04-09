using BussinessLayer.Interfaces.Services;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
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
        [HttpGet]
        [SwaggerOperation(Summary = "Get all users", Description = "Returns all registered users from the database", Tags = new[] { "Users" })]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAllAsync());
        }
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get user", Description = "Returns 1 user from the database", Tags = new[] { "Users" })]
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
        [HttpPost]
        [SwaggerOperation(Summary = "Create user", Description = "Creates a user and sends it to database", Tags = new[] { "Users" })]
        public async Task<IActionResult> CreateAsync([FromBody] cREATEuSERModel us)
        {
            if (us == null)
            {
                return BadRequest("Missing");
            }
            //Console.WriteLine(us);
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
        [SwaggerOperation(Summary = "Change user", Description = "Changes email of a specified user from the database", Tags = new[] { "Users" })]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateModel model)
        {
            if (model == null || id == 0)
            {
                return BadRequest("Something is missing. . .");
            }
            var o = await _userService.GetByIdAsync(id);
            if (o == null) { return NotFound("User not exist"); }
            var user = new UserDTO() { Id = id, Name = o.Name, PublicId = o.PublicId, Emil = model.Email };
            var updated = await _userService.UpdateAsync(user);
            if (!updated)
                return NotFound("Hold up how did ya get here?");
            return Ok(updated);
        }
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a user", Description = "Deletes a specified user from the database", Tags = new[] { "Users" })]
        public async Task<IActionResult> DeleteAsync(int id = 0)
        {
            if (id == 0)
            {
                return BadRequest("User of ID 0 does not exist (or if not given)");
            }
            var o = await _userService.GetByIdAsync(id);
            if (o == null) { return NotFound("User not exist"); }
            bool deleted = await _userService.DeleteAsync(o.PublicId);
            if (!deleted)
                return NotFound("The user does not exist");
            return Ok(deleted);
        }

        /// <summary>
        /// login
        /// </summary>
        private string loggedas = Path.Combine(Directory.GetCurrentDirectory(), "loggedas.json");
        [HttpPost("login")]//Login
        [SwaggerOperation(Summary = "Log in as a user", Description = "Tries to logs u in as a user", Tags = new[] { "Log?" })]
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
                var updatedJson = JsonSerializer.Serialize(
                    login.Name,
                    new JsonSerializerOptions { WriteIndented = true }
                );

                System.IO.File.WriteAllText(loggedas, updatedJson);
                return Ok(q);
            }
        }
        [HttpGet("login")]
        [SwaggerOperation(Summary = "Logged in as?", Description = "Gives u a name of whom u logged as", Tags = new[] { "Log?" })]
        public async Task<IActionResult> LoggedAs()
        {
            var json = System.IO.File.ReadAllText(loggedas);
            var data = JsonSerializer.Deserialize<object>(json) ?? new List<Item>();
            return Ok(data);
        }
        [HttpPost("logof")]
        [SwaggerOperation(Summary = "Logs u off", Description = "What? Idk man...", Tags = new[] { "Log?" })]
        public async Task<IActionResult> LogOf()
        {
            var updatedJson = JsonSerializer.Serialize(
                "",
                new JsonSerializerOptions { WriteIndented = true }
            );
            System.IO.File.WriteAllText(loggedas, updatedJson);
            return Ok(true);
        }

        /// <summary>
        /// store
        /// </summary>
        private string storeC = Path.Combine(Directory.GetCurrentDirectory(), "storecontent.json");
        [HttpGet("storecontent")]
        [SwaggerOperation(Summary = "Get store", Description = "Returns storecontent", Tags = new[] { "Store" })]
        public async Task<IActionResult> GetStore()
        {
            var json = System.IO.File.ReadAllText(storeC);
            var data = JsonSerializer.Deserialize<object>(json) ?? new List<Item>();
            return Ok(data);
        }
        [HttpPost("storecontent")]
        [SwaggerOperation(Summary = "Replace store", Description = "Changes store", Tags = new[] { "Store" })]
        public async Task<IActionResult> ChangeStore([FromBody] List<Item> newData)
        {
            var updatedJson = JsonSerializer.Serialize(
                newData,
                new JsonSerializerOptions { WriteIndented = true }
            );

            System.IO.File.WriteAllText(storeC, updatedJson);

            return Ok(true);
        }
        /// <summary>
        /// Orders
        /// </summary>
        private string OrderL = Path.Combine(Directory.GetCurrentDirectory(), "orderl.json");
        private List<Order> ReadOrders()
        {
            if (!System.IO.File.Exists(OrderL))
                return new List<Order>();
            var json = System.IO.File.ReadAllText(OrderL);
            return string.IsNullOrEmpty(json) ? new List<Order>(): JsonSerializer.Deserialize<List<Order>>(json);
        }
        private void WriteOrders(List<Order> orders)
        {
            var json = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(OrderL, json);
        }

        [HttpGet("order")]
        [SwaggerOperation(Summary = "Get all orders", Description = "Returns all curent orders", Tags = new[] { "Orders" })]
        public IActionResult GetOrders()
        {
            var orders = ReadOrders();
            return Ok(orders);
        }

        [HttpPost("order")]
        [SwaggerOperation(Summary = "Add order", Description = "Adds 1 order", Tags = new[] { "Orders" })]
        public IActionResult AddOrder([FromBody] Order newOrder)
        {
            if (string.IsNullOrWhiteSpace(newOrder.Username) || string.IsNullOrWhiteSpace(newOrder.Items)){
                return BadRequest("Username and Items are required.");
            }
            var orders = ReadOrders();
            var username = newOrder.Username.Trim();
            var existingOrder = orders
                .FirstOrDefault(o => o.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (existingOrder != null){
                existingOrder.Items += ", " + newOrder.Items.Trim();
            }
            else{
                newOrder.Id = orders.Count > 0 ? orders.Max(o => o.Id) + 1 : 1;
                newOrder.Username = username;
                newOrder.Items = newOrder.Items.Trim();
                orders.Add(newOrder);
            }
            WriteOrders(orders);
            return Ok(orders);
        }

        [SwaggerOperation(Summary = "Delete order", Description = "Deletes specified order", Tags = new[] { "Orders" })]
        [HttpDelete("order/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var orders = ReadOrders();
            var order = orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound("Order does not exist/found");

            orders.Remove(order);
            WriteOrders(orders);
            return Ok(order);
        }
    }
    //models
    public class Order
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Items { get; set; } //"burger, fries, cola..."
    }
    public class Item
    {
        public string? Name { get; set; }
    }
}