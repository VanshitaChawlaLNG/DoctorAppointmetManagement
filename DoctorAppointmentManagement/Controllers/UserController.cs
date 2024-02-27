using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using DoctorAppointmentManagement.Models;
using DoctorAppointmentManagement.Services.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointmentManagement.Controllers
{
    [Authorize(Roles ="Patient,Doctors,Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAppointmentServices _appointmentServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,IAppointmentServices AppointmentServices)
        {
            _db = db;
            _userManager = userManager;
            _appointmentServices= AppointmentServices;
        }
        public IActionResult Index()
        {
            IEnumerable<Doctor> obj = _db.Doctors;
            return View(obj);
        }
        public IActionResult Appointment()
        {

            return View();
        }

        [HttpPost]

        [AutoValidateAntiforgeryToken]
        public IActionResult Appointment(Appointment appointment)
        {
            if (appointment == null)
            {
                TempData["ErrorMessage"] = "Missing Entries By The User";
                return View("Error");
            }


            return RedirectToAction("Action");
        }


        public IActionResult GetProfilePicture(string fileName)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "ProfilePictures", fileName);
            return PhysicalFile(filePath, "image/jpeg");
        }
    }
}
