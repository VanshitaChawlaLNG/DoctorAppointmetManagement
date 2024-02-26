using DoctorAppointmentManagement.Data;
using DoctorAppointmentManagement.Models;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            IEnumerable<Doctor> obj = _db.Doctors;
            return View(obj);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AppointmentAdd(int id)
        {
            return AppontmentServices(id); 
        }
        public IActionResult GetProfilePicture(string fileName)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "ProfilePictures", fileName);
            return PhysicalFile(filePath, "image/jpeg");
        }
    }
}
