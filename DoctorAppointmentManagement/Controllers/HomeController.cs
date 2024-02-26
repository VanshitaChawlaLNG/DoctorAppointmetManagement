using DoctorAppointmentManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DoctorAppointmentManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                // Redirect to AdminController's Index action
                return RedirectToAction("Index", "Admin");
            }
            else if (User.IsInRole("Doctor"))
            {
                // Redirect to DoctorsController's Index action
                return RedirectToAction("Index", "Doctor");
            }
            else
            {
                return RedirectToAction("Index", "User");

            }

        }


            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
