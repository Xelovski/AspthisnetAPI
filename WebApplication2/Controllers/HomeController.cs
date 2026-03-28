using System.Diagnostics;
using System.Xml.Linq;
using BussinessLayer.Interfaces.Services;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Datalayer;
using WebApplication2.Datalayer.Entities;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {        
        private static List<UserDTO> _users = new List<UserDTO>();
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IUserService usS)
        {
            _context = context;
            _logger = logger;
            _userService = usS;
        }

        public async Task<IActionResult> Users()
        {
            var q = await _userService.GetAllAsync();
            var userList = await _userService.GetAllPublicIdAsync();
            ViewBag.UserList = "[";
            foreach (var u in userList)
            {
                ViewBag.UserList += $"'{u}'" + ",";
            }
            var a = "";
            a = ViewBag.UserList;
            var i = a.Length;
            ViewBag.UserList = a.Substring(0, i - 1);            
            ViewBag.UserList += "]";
            return View(q);
        }
        [HttpPost]
        public async Task<IActionResult> Users(List<string> name)
        {
            var userList = await _userService.GetAllPublicIdAsync();
            ViewBag.UserList = userList.ToList();            
            foreach (var user in name)
            {
                var a = user.Split(",");
                foreach (var i in a)
                {
                    //Console.WriteLine(i);
                    await _userService.DeleteAsync(new Guid(i));
                }
            }            
            return RedirectToAction("Users");
        }
        public IActionResult Index(){ return View(); }

        public IActionResult Privacy(){ return View(); }
        public async Task<IActionResult> User(Guid userPublicID)//userDetail
        {
            var user = await _userService.GetByPublicIIdAsync(userPublicID);
            var a = new UserEntity()
            {
                Id = user.Id,
                PublicId = user.PublicId,
                Name = user.Name,
                Email = user.Emil
            };
            return View(a);
        }
        public async Task<IActionResult> MakeUser()
        {
            return View(new cREATEuSERModel());
        }
        [HttpPost]
        public async Task<IActionResult> MakeUser(cREATEuSERModel us)
        {
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
            await _userService.Register(l);
            return RedirectToAction("Users");
        }
        public async Task<IActionResult> Update(Guid userPublicID)
        {
            var user = await _userService.GetByPublicIIdAsync(userPublicID);
            return View(new UpdateModel() { PublicId = userPublicID, Email = user.Emil });
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateModel model)
        {
            if (model == null || model.Email == "" || model.Email == null)
            {
                model = new UpdateModel() { PublicId = model.PublicId, Email = "qq@qq.qq" };
            }
            var us = await _userService.GetByPublicIIdAsync(model.PublicId);
            var n = new UserDTO()
            {
                Id = us.Id,
                Emil = model.Email,
                Name = us.Name,
                PublicId = us.PublicId
            };
            await _userService.UpdateAsync(n);
            return RedirectToAction("Users");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Login()
        {
            var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            if (isLoggedIn != "true")
            {
                ViewBag.Name = false;
            }
            else { ViewBag.Name = true; }
                return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string name, string pass)
        {
            var loginDto = new LoginDTO
            {
                Name = name,
                Password = pass
            };
            bool isLoggedIn = await _userService.LoginAsync(loginDto);
            ViewBag.Name = isLoggedIn;
            if (isLoggedIn)
            {                
                HttpContext.Session.SetString("Cart", "");
                HttpContext.Session.SetString("IsLoggedIn", "true");
                HttpContext.Session.SetString("UserName", name);
            }
            else
            {
                HttpContext.Session.SetString("IsLoggedIn", "false");
            }
            return View();
        }
        public async Task<IActionResult> Reset()
        {
            HttpContext.Session.SetString("UserName", "Not Logged");
            HttpContext.Session.SetString("IsLoggedIn", "false");
            HttpContext.Session.SetString("Cart", "");
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Storepage()
        {            
            var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            if (isLoggedIn != "true")
            {
                return RedirectToAction("Login");
            }
            List<string> s = new List<string>
            {
                "Soup",
                "The_picar",
                "Keyboard",
                "Guitar",
                "9mil_(gun)"
            };
            //Console.WriteLine(cart);
            ViewBag.x=s;
            if (HttpContext.Session.GetString("Cart") == null)
            {
                HttpContext.Session.SetString("Cart", "");
            }
            if (HttpContext.Session.GetString("Order") == null)
            {
                HttpContext.Session.SetString("Order", "");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Storepage(string name)
        {
            var o = HttpContext.Session.GetString("Order");
            var u=HttpContext.Session.GetString("UserName");
            var a = HttpContext.Session.GetString("Cart");
            HttpContext.Session.SetString("Cart", ""+a+name);
            HttpContext.Session.SetString("Order", ""+o+";"+u+": "+a+name);
            return RedirectToAction("Checkout");
        }
        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.GetString("Cart") ?? "";
            var grouped = cart
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .GroupBy(x => x)
                .Select(g => new
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .ToList();
            ViewBag.x = grouped;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string name, string actionType)
        {
            var cart = HttpContext.Session.GetString("Cart") ?? "";
            var items = cart
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .ToList();
            var order = HttpContext.Session.GetString("Order") ?? "";
            var qew = order
                .Split(";", StringSplitOptions.RemoveEmptyEntries)
                .ToList();
            if (actionType == "add"){
                items.Add(name);
            }
            else if (actionType == "remove"){
                items.Remove(name); 
            }
            var newCart = string.Join(",", items) + (items.Any() ? "," : "");
            HttpContext.Session.SetString("Cart", newCart);
            ViewBag.x = items
                .GroupBy(x => x)
                .Select(g => new{
                    Name = g.Key,
                    Count = g.Count()
                })
                .ToList();
            return View();
        }
        public async Task<IActionResult> Orders()
        {
            var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            var u = HttpContext.Session.GetString("UserName");
            if (isLoggedIn != "true")
            {
                return RedirectToAction("Login");
            }
            else if (u == "a") //admin
            {
                var cart = HttpContext.Session.GetString("Order") ?? "";
                var items = cart
                    .Split(";", StringSplitOptions.RemoveEmptyEntries)
                    .ToList();                
                ViewBag.x = items.ToList();
            }
            else // user
            {
                var cart = HttpContext.Session.GetString("Cart") ?? "";
                var items = cart
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .ToList();


                var order = HttpContext.Session.GetString("Order") ?? "";
                var userOrder = order 
                    .Split(";", StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault(o => o.StartsWith(u + ":"))?? " ";
                var q = userOrder
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                var orderedItems = new List<string>();
                if (userOrder != null)
                {
                    orderedItems = userOrder
                        .Substring(u.Length + 1)
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .ToList();
                }
                ViewBag.x = q.ToList();
            }
            return View();
        }

        public async Task<IActionResult> Ch()
        {
            var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            var u = HttpContext.Session.GetString("UserName");
            if (isLoggedIn != "true")
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public async Task<IActionResult> Rs()
        {
            return View();
        }












































        //Henlo what you doing down here




    }
}
