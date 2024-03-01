using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using DoctorAppointmentManagement.Models;
using DoctorAppointmentManagement.Services.AddTimingData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;  


namespace DoctorAppointmentManagement.Controllers
{
    [Authorize(Roles = "Doctors")]
    public class DoctorController : Controller
    {
        private readonly ITiming _timingServices;
       
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(ITiming timingServices, UserManager<ApplicationUser> userManager,ILogger<DoctorController> logger)
        {
            _timingServices = timingServices;
            _logger = logger;
            _userManager = userManager;  
        }

		[HttpGet]
		public IActionResult AddTiming()
		{
			return View();
		}

		[HttpPost]
		[AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddTimingpost(AvailableTiming availableTiming)
        {
            try
            {
                if (availableTiming == null)
                {
                    ModelState.AddModelError("Name", "The fields are not correct. Check again.");
                    return View();
                }

                ModelState.Clear();

                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        TempData["ErrorMessage"] = "Failed to Fetch User. Please try again later.";
                        return View();
                    }

                    var timingResult = await _timingServices.AddAvailableTimings(availableTiming, user);

                    if (timingResult is OkResult)
                    {
                        TempData["SuccessMessage"] = "Timing added successfully.";
                        return RedirectToAction("AddTiming");
                    }

                    TempData["ErrorMessage"] = "Failed to add timing. Please try again later.";
                    return View();
                }

            
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["ErrorMessage"] = $"Invalid model state. Please check your input. Errors: {string.Join(", ", errorMessages)}";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception at Timing Adding");
                Console.WriteLine($"An error occurred: {ex.Message}");
                TempData["ErrorMessage"] = "An unexpected error occurred. Please contact support.";
                return View();
            }
        }


        public async Task<IActionResult> ShowAppointsToDoctor()
        {
            try
            {
                var doctorUser = await _userManager.GetUserAsync(User);

                if (doctorUser != null)
                {
                    var doctorAppointments = await _timingServices.ShowBookinkToDoctor(doctorUser);
                    return View(doctorAppointments);
                }
                TempData["ErrorMessage"] = "Can't Fetch Doctor";

                return RedirectToAction("Index","Home");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An Exception occurred at Showing Appointments of Doctor: {ex.Message}");
                TempData["ErrorMessage"] = "Exception";
               
                return RedirectToAction("Index","Home");
            }
        }

    }
}

