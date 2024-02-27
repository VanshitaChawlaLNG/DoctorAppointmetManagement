using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using DoctorAppointmentManagement.Services.AppointmentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentManagement.Controllers
{
    [Authorize(Roles = "Patient,Doctors,Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAppointmentService _appointmentService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IAppointmentService AppointmentServices)
        {
            _db = db;
            _userManager = userManager;
            _appointmentService = AppointmentServices;
        }
        public IActionResult Index()
        {
            IEnumerable<Doctor> obj = _db.Doctors;
            return View(obj);
        }
        public IActionResult Appointment(int DoctorId)
        {
            int doctorId = DoctorId;

            var timingSlots = _db.TimingSlots
       .Include(ts => ts.Slots)
       .Where(ts => ts.DoctorId == doctorId)
       .ToList();


            ViewBag.TimingSlots = timingSlots;
            return View();
        }

        [HttpPost]

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Appointment(Appointment appointment,int DoctorId)
        {
            try
            {
                if (appointment == null)
                {

                    TempData["ErrorMessage"] = "Missing Entries By The User";
                    return View("Error");
                }
                var user =await  _userManager.GetUserAsync(User);
                if (ModelState.IsValid)
                {
                    var result = await _appointmentService.DoctorAppointment(appointment, user, DoctorId);
                }


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

                TempData["ErrorMessage"] = "{ex.Message}.";
                return View("Error");
            }
        }


        public IActionResult GetProfilePicture(string fileName)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "ProfilePictures", fileName);
            return PhysicalFile(filePath, "image/jpeg");
        }
    }
}
