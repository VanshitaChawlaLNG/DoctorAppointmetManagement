using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using DoctorAppointmentManagement.Services.AppointmentServices;
using DoctorAppointmentManagement.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentManagement.Controllers
{
    [Authorize(Roles = "Patient")]
    public class UserController : Controller
    {
       
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController( UserManager<ApplicationUser> userManager, IAppointmentService AppointmentServices,IUserService userService)
        {
            _userService = userService;
            _userManager = userManager;
            _appointmentService = AppointmentServices;
        }
        public async Task<IActionResult> Index()
        {
           var obj=await _userService.ShowDoctorsAdded();
            return View(obj);
        }
     
        [HttpGet]
        public async Task<IActionResult> Appointment(int DoctorId)
        {
            try
            {
                var timingSlots = await _userService.FetchTimingsToShow(DoctorId);

                if (timingSlots != null && timingSlots.Any())
                {
                    ViewBag.TimingSlotsList = new SelectList(timingSlots, timingSlots);
                    return View();
                }

                TempData["ErrorMessage"] = "No timing slots available for the selected doctor.";
                return View("Error");
            }
            catch (Exception ex)
            {
               
                TempData["ErrorMessage"] = "Failed to fetch timing slots. Please try again later.";
                return View("Error");
            }
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Appointment(Appointment appointment)
        {
            try
            {
                ModelState.Clear();
                if (!ModelState.IsValid || appointment == null)
                {
                    TempData["ErrorMessage"] = "Invalid or Missing Entries By The User";
                    return View();
                }

                var selectedValue = Request.Form["Timestamp"].FirstOrDefault();
               // appointment.Timestamp = selectedValue;

                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    TempData["ErrorMessage"] = "Failed to Fetch User. Please try again later.";
                    return RedirectToAction("ShowAppointments");
                }

                var result = await _appointmentService.DoctorAppointment(appointment, user);

                if (result)
                {
                    TempData["SuccessMessage"] = "Data Inserted";
                    return RedirectToAction("ShowAppointments");
                }

                TempData["ErrorMessage"] = "Failed to Insert Data. Please try again.";
                return RedirectToAction("ShowAppointments"); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return View();
            }
        }

        public async Task<IActionResult> ShowAppointments()
        {
            try
            {
                // Get the currently logged-in user
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    // Fetch appointments for the logged-in user
                   var appointments=  _userService.fetchAppointments(user);

                    // Pass appointments to the view
                    ViewBag.Appointments = appointments;

                    return View("Admin/ShowAppointments");
                }

                TempData["ErrorMessage"] = "User not found.";
                return View("Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                TempData["ErrorMessage"] = $"{ex.Message}.";
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
