using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class UlogaController : Controller
    {
        private static List<Useri> usersL { get; set; } = new List<Useri>();
        public IActionResult Uloha()
        {
            return View();
        }
        public IActionResult Uloha2()
        {
            return View();
        }
        public IActionResult Uloha3()
        {
            return View();
        }
        public IActionResult Uloha4()
        {
            /*
            List<Useri> users = new List<Useri>()
            {
                new Useri() {
                    Name = "TesN",
                    Email = "Test@",
                    Password = "12345",
                    Username = "TestUn",
                },
                new Useri() {
                    Name = "TesN1",
                    Email = "Test@1",
                    Password = "123451",
                    Username = "TestUn1",
                }
            };
            return View(users);*/
            return View();
        }
        public IActionResult Uloha5()
        {
            /*
            List<Useri> users = new List<Useri>()
            {
                new Useri() {
                    Name = "TesN",
                    Email = "Test@",
                    Password = "12345",
                    Username = "TestUn",
                },
                new Useri() {
                    Name = "TesN1",
                    Email = "Test@1",
                    Password = "123451",
                    Username = "TestUn1",
                },
                new Useri()
                {
                    Name="Steph"
                    ,Email="seph@special.com"
                    ,Password="1sd4"
                    ,Username="StepS"
                }
                ,new Useri()
                {
                    Name="someoneElse"
                    ,Email="someone@else.com"
                    ,Password="4"
                    ,Username="Heeeeeeeee"
                }
            };
            return View(users);*/return View();
        }
        public IActionResult Uloha6()
        {
            return View();
        }
        public IActionResult Uloha7()
        {
            return View();
        }
        public IActionResult Uloha8()
        {
            return View();
        }
        public IActionResult Uloha9()
        {

            return View();
        }
        public IActionResult U10()
        {
            return View(new Useri());
        }
        [HttpPost]
        public IActionResult U10(Useri u)
        {
            usersL.Add(u);
            return RedirectToAction("U10out");
        }
        [HttpGet]
        public IActionResult U10out() 
        {
            ViewBag.X = usersL;
            return View(); 
        }
    }
}
