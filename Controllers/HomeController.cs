using backend_property_list.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace backend_property_list.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Welcome to the Home Page!");
        }
    }

}
